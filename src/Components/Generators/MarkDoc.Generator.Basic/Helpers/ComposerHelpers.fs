namespace MarkDoc.Generator.Basic

open MarkDoc.Elements
open MarkDoc.Members.Types
open System
open MarkDoc.Documentation.Tags
open MarkDoc.Members.Members
open System.Collections.Generic
open MarkDoc.Members.Enums
open MarkDoc.Members
open MarkDoc.Helpers

module internal ComposerHelpers =
  let createHeadings headings tools = headings |> Seq.map (fun x -> TextHelpers.normal x tools)

  let methodArguments source (item: IMember) tools =
    let argument arg =
      JoinedText ([ arg |> StringConverters.argumentTypeStr |> Normal; TypeHelpers.processResType source arg.Type tools; Normal arg.Name ], " ")
    let processArguments args =
      JoinedText (args |> Seq.map argument, ", ")

    match item with 
    | :? IConstructor as c -> c.Arguments |> processArguments
    | :? IDelegate as d -> d.Arguments |> processArguments
    | _ -> raise (Exception())

  let memberNameSummary(input: IType, name: TextType<ILink>, summary: Option<ITag>, tools) =
    match summary with
    | None -> name
    | Some x -> JoinedText (seq [ name; TagHelpers.tagShort(input, x, tools) ], Environment.NewLine)

  let registerSection (input, name, content) tools level =
    tools.linker.RegisterAnchor(input, lazy(name))
    ElementHelpers.initialize ((content, name, level) |> Section) tools

  let composeSections input level =
    let createSection (x, y) =
      Element.Section(x, y, level)

    input
    |> SomeHelpers.whereSome2
    |> Seq.map (SomeHelpers.get2 >> createSection >> ElementHelpers.initialize)

  let printIntroduction input =
    Seq.empty |> Some
  let printMemberTables input =
    Seq.empty |> Some
  let printDetailed(input: IType, tools) =
    let applyTools input = input tools
    let joinContentWSig content signature (mem: IMember) =
      let applyMember input = input mem
      let processContent (input: (IMember -> IElement seq option) * string) =
        (input |> fst |> applyMember, snd input)

      composeSections (content |> Seq.map processContent) 4
      |> Seq.map (applyTools >> ElementHelpers.toElement)
      |> Seq.append (seq [ signature |> applyMember |> TextHelpers.processText |> applyTools |> ElementHelpers.toElement ])
    let single x =
      let tag = TagHelpers.findTypeTag(input, x, tools) |> Seq.tryExactlyOne
      if Option.isNone tag then
        None
      else
        TagHelpers.tagFull(input, tag |> Option.get, tools) |> SomeHelpers.emptyToNone
    let genericProcessor provider processItem =
      let processItems (items: 'M IReadOnlyCollection when 'M :> IMember) =
        items
        |> Seq.mapi (fun x y -> registerSection (processItem(x, y)) tools 3 |> ElementHelpers.toElement)

      if Seq.isEmpty provider then
        None
      else
        provider
        |> processItems
        |> SomeHelpers.emptyToNone

    let nested =
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

    let inheritance =
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

    let typeParams =
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

    let getSingleTag t m =
      let single tags =
        match tags with
        | Some tag -> TagHelpers.tagFull(input, tag, tools) |> Seq.map ElementHelpers.toElement |> SomeHelpers.emptyToNone
        | _ -> None
      TagHelpers.findTag(input, m, t, tools)
      |> Seq.tryExactlyOne
      |> single
    let getExceptions m =
      let exceptions = TagHelpers.findTag(input, m, ITag.TagType.Exception, tools)
                       |> Seq.map (fun x -> seq [ TypeHelpers.processReference(input, x.Reference, tools); TagHelpers.tagShort(input, x, tools) ] |> Seq.map (TextHelpers.processText >> applyTools >> ElementHelpers.toElement) |> Linq.ToReadOnlyCollection)

      if Seq.isEmpty exceptions then
        None
      else
        seq [ tools.creator.CreateTable(exceptions, createHeadings (seq [ "Name"; "Description" ]) tools) |> ElementHelpers.toElement ] |> Some
    let getSeeAlso m =
      let seeAlsos = TagHelpers.findTag(input, m, ITag.TagType.Seealso, tools)
                     |> Seq.map (fun x -> TagHelpers.tagShort(input, x, tools) |> TextHelpers.processText |> applyTools |> ElementHelpers.toElement)

      if Seq.isEmpty seeAlsos then
        None
      else
        seq [ tools.creator.CreateList(seeAlsos, IList.ListType.Dotted) |> ElementHelpers.toElement ] |> Some
    let getArguments (c: IMember) = 
      let argumentDocs = TagHelpers.findTag(input, c, ITag.TagType.Param, tools)
                         |> Seq.map (fun x -> x.Reference, TagHelpers.tagShort(input, x, tools) |> TextHelpers.processText |> applyTools |> ElementHelpers.toElement)
                         |> dict

      let processArguments (x: IArgument) =
        let getDescription =
          let mutable value: IElement = null
          seq [ if argumentDocs.TryGetValue(x.Name, &value) then yield value ]

        let argType = 
          let argType = seq [
            let argType = x |> StringConverters.argumentTypeStr
            if not (String.IsNullOrEmpty argType) then
              yield argType |> InlineCode
          ]

          JoinedText (seq [ TypeHelpers.processResType input x.Type tools ] |> Seq.append argType, " ")

        let typeName = seq [ argType; x.Name |> Normal ]
                       |> Seq.map (TextHelpers.processText >> applyTools >> ElementHelpers.toElement)
        getDescription
        |> Seq.append typeName
        |> Linq.ToReadOnlyCollection

      let generateResult (args: IArgument seq) =
        let arguments = args
                        |> Seq.map processArguments
        if (Seq.isEmpty args || Seq.isEmpty arguments) then
          None
        else
          seq [ tools.creator.CreateTable(arguments, createHeadings (seq [ "Type"; "Name"; "Description" ]) tools) |> ElementHelpers.toElement ] |> Some

      match c with
      | :? IConstructor as x -> generateResult x.Arguments
      | :? IDelegate as x -> generateResult x.Arguments
      | _ -> raise (Exception())
    let getInheritedFrom m = 
      let getInheritance(x: IInterface) =
        let typeReference (t: IType) = 
          (TypeHelpers.getTypeName t |> Normal, tools.linker.CreateLink(input, t)) |> LinkElement

        let mutable result: IInterface = null
        if x.InheritedTypes.Value.TryGetValue(m, &result) then
          Some(seq [ typeReference result |> ElementHelpers.initialize |> applyTools ])
        else
          None

      match input with
      | :? IInterface as i -> getInheritance i
      | _ -> None
    let overloads (members: 'M IReadOnlyCollection when 'M :> IMember) =
      members
      |> Seq.groupBy (fun x -> x.Name)
      |> Seq.map (fun x -> (fst x, snd x |> Seq.mapi (fun x y -> (y.RawName, x)) |> dict))
      |> dict
    let overloadFormat overloadCount overloadIndex =
      if overloadCount > 1 then
        String.Format(" [{0}/{1}]", overloadIndex + 1, overloadCount)
      else
        String.Empty

    let getArgumentsSig (mem: IMember) =
      methodArguments input mem tools |> TextHelpers.processTextNoStyle

    let constructors =
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
            getArgumentsSig
          ])

        let content =
          seq [
            (getArguments, "Arguments")
            (getSingleTag ITag.TagType.Summary, "Summary")
            (getSingleTag ITag.TagType.Remarks, "Remarks")
            (getSingleTag ITag.TagType.Example, "Example")
            (getExceptions, "Exceptions")
            (getSeeAlso, "See also")
          ]

        (ctor, ctor.Name + overloadFormat extractor.Count i, joinContentWSig content signature ctor)

      genericProcessor extractor processCtor

    let methods =
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
            getArgumentsSig
          ])

        let content =
          seq [
            (getArguments, "Arguments")
            (getSingleTag ITag.TagType.Summary, "Summary")
            (getSingleTag ITag.TagType.Remarks, "Remarks")
            (getSingleTag ITag.TagType.Example, "Example")
            (getSingleTag ITag.TagType.Returns, "Returns")
            (getExceptions, "Exceptions")
            (getInheritedFrom, "Inherited from")
            (getSeeAlso, "See also")
          ]

        (method, (if method.Operator <> OperatorType.None then "Operator " else "") + method.Name + getOverloads, joinContentWSig content signature method)

      genericProcessor extractor processMethod

    let properties =
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
            (getSingleTag ITag.TagType.Summary, "Summary")
            (getSingleTag ITag.TagType.Remarks, "Remarks")
            (getSingleTag ITag.TagType.Value, "Value")
            (getSingleTag ITag.TagType.Example, "Example")
            (getExceptions, "Exceptions")
            (getInheritedFrom, "Inherited from")
            (getSeeAlso, "See also")
          ]

        (property, property.Name, joinContentWSig content signature property)

      genericProcessor extractor processProperty

    let events =
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
            (getSingleTag ITag.TagType.Summary, "Summary")
            (getSingleTag ITag.TagType.Remarks, "Remarks")
            (getSingleTag ITag.TagType.Example, "Example")
            (getExceptions, "Exceptions")
            (getInheritedFrom, "Inherited from")
            (getSeeAlso, "See also")
          ]

        (event, event.Name, joinContentWSig content signature event)

      genericProcessor extractor processEvent

    let delegates =
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
            getArgumentsSig
          ])

        let content =
          seq [
            (getArguments, "Arguments")
            (getSingleTag ITag.TagType.Summary, "Summary")
            (getSingleTag ITag.TagType.Remarks, "Remarks")
            (getSingleTag ITag.TagType.Example, "Example")
            (getSingleTag ITag.TagType.Returns, "Returns")
            (getInheritedFrom, "Inherited from")
            (getSeeAlso, "See also")
          ]

        (deleg, deleg.Name + getOverloads, joinContentWSig content signature deleg)

      genericProcessor extractor processDelegate

    let enumFields =
      let extractor =
        match input with
        | :? IEnum as e -> e.Fields
        | _ -> new LinkedList<IEnumField>() :> IReadOnlyCollection<IEnumField>
      let processField (_, field: IEnumField) =
        let content =
          (seq [
            (getSingleTag ITag.TagType.Summary field, "Summary")
            (getSingleTag ITag.TagType.Remarks field, "Remarks")
            (getSingleTag ITag.TagType.Example field, "Example")
            (getSingleTag ITag.TagType.Returns field, "Returns")
            (getSeeAlso field, "See also")
          ]
          |> composeSections) 4
          |> Seq.map applyTools

        (field, field.Name, (if Seq.isEmpty content then seq [ TextHelpers.empty tools |> ElementHelpers.toElement ] else content))

      genericProcessor extractor processField

    let sections =
      seq [
        (single ITag.TagType.Summary, "Summary");
        (single ITag.TagType.Remarks, "Remarks");
        (single ITag.TagType.Example, "Example");
        (typeParams, "Generic types");
        (inheritance, "Inheritance");
        (nested, "Nested types")
        (single ITag.TagType.Seealso, "See also")
        (constructors, "Constructors")
        (methods, "Methods")
        (properties, "Properties")
        (events, "Events")
        (delegates, "Delegates")
        (enumFields, "Fields")
      ]
      |> composeSections

    sections 2 |> SomeHelpers.emptyToNone

  let composeContent input tools =
    (seq [
       (printIntroduction input, "Description");
       (printMemberTables input, "Members");
       (printDetailed(input, tools), "Details")
     ]
     |> composeSections) 1
     |> Seq.map (fun x -> x tools)

