namespace MarkDoc.Generator.Basic

open MarkDoc.Members.Types
open MarkDoc.Elements
open MarkDoc.Documentation.Tags
open System
open MarkDoc.Members.Members
open MarkDoc.Members.Enums
open System.Collections.Generic
open MarkDoc.Members
open MarkDoc.Helpers

type TypeContentType =
  | TypeSummary
  | TypeRemarks
  | TypeExample
  | TypeGenerics
  | TypeInheritance
  | TypeNested
  | TypeSeeAlso
  | TypeConstructors
  | TypeMethods
  | TypeProperties
  | TypeEvents
  | TypeDelegates
  | TypeFields

module TypeContentHelpers =
  let private createHeadings headings tools = headings |> Seq.map (fun x -> TextHelpers.normal x tools)
  let registerSection (input, name, content) tools level =
    tools.linker.RegisterAnchor(input, lazy(name))
    ElementHelpers.initialize ((content, name, level) |> Section) tools

  let composeSections input level =
    let createSection (x, y) =
      Element.Section(x, y, level)

    input
    |> SomeHelpers.whereSome2
    |> Seq.map (SomeHelpers.get2 >> createSection >> ElementHelpers.initialize)
  let methodArguments source (item: IMember) tools =
    let argument arg =
      JoinedText ([ arg |> StringConverters.argumentTypeStr |> Normal; TypeHelpers.processResType source arg.Type tools; Normal arg.Name ], " ")
    let processArguments args =
      JoinedText (args |> Seq.map argument, ", ")

    match item with 
    | :? IConstructor as c -> c.Arguments |> processArguments
    | :? IDelegate as d -> d.Arguments |> processArguments
    | _ -> raise (Exception())

  let private single x input tools =
    let tag = TagHelpers.findTypeTag(input, x, tools) |> Seq.tryExactlyOne
    if Option.isNone tag then
      None
    else
      TagHelpers.tagFull(input, tag |> Option.get, tools) |> SomeHelpers.emptyToNone
  let private genericProcessor provider processItem tools =
    let processItems (items: 'M IReadOnlyCollection when 'M :> IMember) =
      items
      |> Seq.mapi (fun x y -> registerSection (processItem(x, y)) tools 3 |> ElementHelpers.toElement)

    if Seq.isEmpty provider then
      None
    else
      provider
      |> processItems
      |> SomeHelpers.emptyToNone
  let private processContent (item: (IMember -> Tools -> IElement seq option) * string) (mem: IMember) tools =
    let applyMember input = input mem
    let applyTools input = input tools
    (item |> (fst >> applyMember >> applyTools), snd item)

  let private joinContentWSig content signature (mem: IMember) (input: IType) tools =
    let applyMember input = input mem
    let applyTools input = input tools
    let processContents (content: IType -> seq<(IMember -> Tools -> IElement seq option) * string>) =
      content input |> Seq.map processContent

    let toProcess = processContents (content |> ContentHelpers.processContents)
                    |> Seq.map (applyMember >> applyTools)

    let a = composeSections toProcess 4
            |> Seq.map (applyTools >> ElementHelpers.toElement)
            |> Seq.append (seq [ signature |> applyMember |> TextHelpers.processText |> applyTools |> ElementHelpers.toElement ])
    a

  let private overloads (members: 'M IReadOnlyCollection when 'M :> IMember) =
    members
    |> Seq.groupBy (fun x -> x.Name)
    |> Seq.map (fun x -> (fst x, snd x |> Seq.mapi (fun x y -> (y.RawName, x)) |> dict))
    |> dict
  let private overloadFormat overloadCount overloadIndex =
    if overloadCount > 1 then
      String.Format(" [{0}/{1}]", overloadIndex + 1, overloadCount)
    else
      String.Empty
  let private getArgumentsSig (input: IType) tools (mem: IMember) =
    methodArguments input mem tools |> TextHelpers.processTextNoStyle

  let private nested (input: IType) tools =
    let applyTools input = input tools
    let getNested (x: IInterface) = x.NestedTypes

    let groupByType (x: IType) =
      match x with
      | :? IClass
        -> "c" |> Some
      | :? IStruct
        -> "s" |> Some
      | :? IInterface
        -> "i" |> Some
      | :? IEnum
        -> "e" |> Some
      | _ -> None

    let processGroup (x: string option * seq<IType>) =
      let createTable heading group =
        ListElement (group |> Seq.map (fun x -> ElementHelpers.initialize (TypeHelpers.getTypeName x |> InlineCode |> TextElement) tools), IList.ListType.Dotted, heading, 3)
        |> Some

      match x |> (fst >> Option.get) with
      | "c" -> createTable "Classes" (snd x)
      | "i" -> createTable "Interfaces" (snd x)
      | "s" -> createTable "Structures" (snd x)
      | "e" -> createTable "Enums" (snd x)
      | _ -> None

    match input with
    | :? IInterface as x ->
       x
       |> getNested
       |> Seq.groupBy groupByType
       |> Seq.filter (fst >> Option.isSome)
       |> Seq.map processGroup
       |> SomeHelpers.whereSome
       |> Seq.map (ElementHelpers.initialize >> applyTools)
       |> SomeHelpers.emptyToNone
    | _ -> None

  let private inheritance (input: IType) tools =
    let getInterfaces (x: 'M when 'M :> IInterface) =
      x.InheritedInterfaces
      |> Seq.map (fun x -> ElementHelpers.initialize (TypeHelpers.processResType input x tools |> TextElement) tools)

    let createList l =
      if (Seq.isEmpty l) then
        None
      else
        seq [ tools.creator.CreateList(l, IList.ListType.Dotted) |> ElementHelpers.toElement ] |> Some

    match input with
    | :? IClass as x ->
      let baseType = if (isNull x.BaseClass) then None else ElementHelpers.initialize (TypeHelpers.processResType input x.BaseClass tools |> TextElement) tools |> Some
      let interfaces = getInterfaces x
      seq [ baseType ]
      |> SomeHelpers.whereSome
      |> Seq.append interfaces
      |> createList
    | :? IInterface as x ->
      x |> (getInterfaces >> createList)
    | _ -> None

  let private typeParams (input: IType) tools =
    let applyTools input = input tools
    let getTypeParams =
      let processTag (x: ITag) =
        let generics = (input :?> IInterface).Generics
        let getConstraints (x: ITag) =
          if generics.ContainsKey(x.Reference) then
            let types = generics.[x.Reference].ToTuple()
                        |> snd
                        |> Seq.map (fun x -> TypeHelpers.processResType input x tools)
            (types, Environment.NewLine) |> JoinedText |> Some
          else
            None

        let getName (x: ITag) =
          let result = seq [
            yield x.Reference |> InlineCode
            if (generics.ContainsKey(x.Reference)) then
              let variance = generics.[x.Reference].ToTuple() |> fst
              if (variance <> Enums.Variance.NonVariant) then
                yield variance |> StringConverters.varianceStr |> InlineCode
          ]
          (result, " ") |> JoinedText

        let constraints = getConstraints x
        seq [
          yield getName x
          yield TagHelpers.tagShort(input, x, tools)

          if (Option.isSome constraints) then
            yield constraints |> Option.get
        ]
        |> Seq.map (TextHelpers.processText >> applyTools >> ElementHelpers.toElement)

      if input :? IInterface then
        TagHelpers.findTypeTag(input, ITag.TagType.Typeparam, tools)
        |> Seq.map (processTag >> Linq.ToReadOnlyCollection)
        |> Some
      else
        None

    let ts = getTypeParams
    if (Option.isSome ts && ts |> (Option.get >> Seq.isEmpty >> not)) then
      seq [ tools.creator.CreateTable(ts |> Option.get, createHeadings (seq [ "Type"; "Description"; "Constraints" ]) tools) |> ElementHelpers.toElement ] |> Some
    else
      None

  let private constructors (input: IType) tools =
    let extractor =
      match input with
      | :? IClass as x -> x.Constructors
      | _ -> LinkedList<IConstructor>() :> IReadOnlyCollection<IConstructor>
    let processCtor (i: int, ctor: IConstructor) =
      let signature =
        SignatureHelpers.generateSignature "{0}{1} {2}({{3}})" (seq [
          SignatureHelpers.getAccessor;
          SignatureHelpers.getStatic;
          SignatureHelpers.getName;
          getArgumentsSig input tools
        ])

      let content =
        seq [
          Arguments;
          Summary;
          Remarks;
          Example;
          Exceptions;
          SeeAlso
        ]

      (ctor, ctor.Name + overloadFormat extractor.Count i, joinContentWSig content signature ctor input tools)

    genericProcessor extractor processCtor tools

  let private methods (input: IType) tools =
    let extractor =
      match input with
      | :? IInterface as x -> x.Methods
      | _ -> new LinkedList<IMethod>() :> IReadOnlyCollection<IMethod>
    let processMethod (_, method: IMethod) =
      let getOverloads = 
        let overloads = (overloads extractor).[method.Name]
        overloadFormat overloads.Count overloads.[method.RawName]

      let signature =
        SignatureHelpers.generateSignature "{0}{1}{2} {3}{4}{5} {6}{7}({8})" (seq [
          SignatureHelpers.getAccessor;
          SignatureHelpers.getStatic;
          SignatureHelpers.getInheritance;
          SignatureHelpers.getAsync;
          SignatureHelpers.getReturn;
          SignatureHelpers.getOperator;
          SignatureHelpers.getName;
          SignatureHelpers.getGenerics;
          getArgumentsSig input tools
        ])

      let content =
        seq [
          Arguments;
          Summary;
          Remarks;
          Example;
          Returns;
          Exceptions;
          Inheritance;
          SeeAlso
        ]

      (method, (if method.Operator <> OperatorType.None then "Operator " else "") + method.Name + getOverloads, joinContentWSig content signature method input tools)

    genericProcessor extractor processMethod tools

  let private properties (input: IType) tools =
    let extractor = 
      match input with
      | :? IInterface as x -> x.Properties
      | _ -> new LinkedList<IProperty>() :> IReadOnlyCollection<IProperty>
    let processProperty (_, property: IProperty) =
      let signature =
        SignatureHelpers.generateSignature "{0}{1}{2} {3} {4} {{ {5} }}" (seq [
          SignatureHelpers.getAccessor;
          SignatureHelpers.getStatic;
          SignatureHelpers.getInheritance;
          SignatureHelpers.getReturn;
          SignatureHelpers.getName;
          SignatureHelpers.getPropertyMethods
        ])
      let content =
        seq [
          Summary;
          Remarks;
          Value;
          Example;
          Exceptions;
          Inheritance;
          SeeAlso
        ]

      (property, property.Name, joinContentWSig content signature property input tools)

    genericProcessor extractor processProperty tools

  let private events (input: IType) tools =
    let extractor = 
      match input with
      | :? IInterface as x -> x.Events
      | _ -> new LinkedList<IEvent>() :> IReadOnlyCollection<IEvent>
    let processEvent (_, event: IEvent) =
      let signature =
        SignatureHelpers.generateSignature "{0}{1} event {2} {3}" (seq [
          SignatureHelpers.getAccessor;
          SignatureHelpers.getStatic;
          SignatureHelpers.getReturn;
          SignatureHelpers.getName
        ])
      let content =
        seq [
          Summary;
          Remarks;
          Example;
          Exceptions;
          Inheritance;
          SeeAlso
        ]

      (event, event.Name, joinContentWSig content signature event input tools)

    genericProcessor extractor processEvent tools

  let private delegates (input: IType) tools =
    let extractor = 
      match input with
      | :? IInterface as x -> x.Delegates
      | _ -> new LinkedList<IDelegate>() :> IReadOnlyCollection<IDelegate>
    let processDelegate (_, deleg: IDelegate) =
      let getOverloads = 
        let overloads = (overloads extractor).[deleg.Name]
        overloadFormat overloads.Count overloads.[deleg.RawName]

      let signature =
        SignatureHelpers.generateSignature "{0}{1} delegate {2} {3}{4}({5})" (seq [
          SignatureHelpers.getAccessor;
          SignatureHelpers.getStatic;
          SignatureHelpers.getReturn;
          SignatureHelpers.getName;
          SignatureHelpers.getGenerics;
          getArgumentsSig input tools
        ])

      let content =
        seq [
          Arguments;
          Summary;
          Remarks;
          Example;
          Returns;
          Inheritance;
          SeeAlso
        ]

      (deleg, deleg.Name + getOverloads, joinContentWSig content signature deleg input tools)

    genericProcessor extractor processDelegate tools

  let private enumFields (input: IType) tools =
    let extractor =
      match input with
      | :? IEnum as e -> e.Fields
      | _ -> new LinkedList<IEnumField>() :> IReadOnlyCollection<IEnumField>
    let processField (_, field: IEnumField) =
      let applyTools input = input tools
      let content =
        let sections =
          seq [
            Summary;
            Remarks;
            Example;
            Returns;
            SeeAlso
          ]
        let processed = ContentHelpers.processContents sections input
                        |> Seq.map (fun x -> (processContent x field) tools)

        composeSections processed 4
        |> Seq.map applyTools

      (field, field.Name, (if Seq.isEmpty content then seq [ TextHelpers.empty tools |> ElementHelpers.toElement ] else content))

    genericProcessor extractor processField tools

  let processContents input tools content =
    let processContent content =
      match content with
      | TypeSummary as s ->
        (single ITag.TagType.Summary input tools, "Summary")
      | TypeRemarks as s ->
        (single ITag.TagType.Remarks input tools, "Remarks")
      | TypeExample as s ->
        (single ITag.TagType.Example input tools, "Example")
      | TypeGenerics as s ->
        (typeParams input tools, "Generic types")
      | TypeInheritance as s ->
        (inheritance input tools, "Inheritance")
      | TypeNested as s ->
        (nested input tools, "Nested types")
      | TypeSeeAlso as s ->
        (single ITag.TagType.Seealso input tools, "See also")
      | TypeConstructors as s ->
        (constructors input tools, "Constructors")
      | TypeMethods as s ->
        (methods input tools, "Methods")
      | TypeProperties as s ->
        (properties input tools, "Properties")
      | TypeEvents as s ->
        (events input tools, "Events")
      | TypeDelegates as s ->
        (delegates input tools, "Delegates")
      | TypeFields as s ->
        (enumFields input tools, "Fields")

    content |> Seq.map processContent
