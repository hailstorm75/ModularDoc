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
open MarkDoc.Members.ResolvedTypes
open MarkDoc.Documentation

module internal ComposerHelpers =
  let createHeadings headings tools = headings |> Seq.map (fun x -> TextHelpers.normal x tools)

  let getTypeName (input: IType) =
    let joinGenerics (i: seq<string>) =
      let partial f x y = f(x , y)
      let generics = i |> partial String.Join ", "
      "<" + generics + ">"
    let processInterface (input: IInterface) =
      let generics =
        input.Generics
        |> Seq.map (fun x -> (x.Value.ToTuple() |> fst |> StringConverters.varianceStr) + " " + x.Key)
      if not (Seq.isEmpty generics) then
        input.Name + joinGenerics generics
      else
        input.Name
    let processStruct (input: 'M when 'M :> IInterface) =
      let generics =
        input.Generics
        |> Seq.map (fun x -> x.Key)
      if not (Seq.isEmpty generics) then
        input.Name + joinGenerics generics
      else
        input.Name

    match input with
    | :? IClass as x -> processStruct x
    | :? IStruct as x -> processStruct x
    | :? IInterface as x -> processInterface x
    | _ -> input.Name

  let tryFindMember (input: IType, memberFull: string, memberCut: string) =
    let toMember (a: 'M when 'M :> IMember) =
      a :> IMember
    let findMember (a: 'M seq when 'M :> IMember) =
      a |> Seq.map toMember |> Seq.tryFind (fun x -> x.Name.Equals(memberCut))
    match input with
    | :? IInterface as i ->
      match memberFull.[0] with
      | 'M' -> i.Methods |> findMember
      | 'P' -> i.Properties |> findMember
      | 'E' -> i.Events |> findMember
      | _ -> None
    | :? IEnum as e -> e.Fields |> findMember
    | _ -> None

  let processReference (input: IType, reference: string, tools) =
    let typeReference (reference: string) =
      let mutable result: IType = null
      if tools.typeResolver.TryFindType(reference.[2..], &result) then
        LinkContent ((getTypeName result) |> Normal, lazy(tools.linker.CreateLink(input, result)))
      else
        let slice = reference.AsSpan(reference.LastIndexOf('.') + 1)
        let index = slice.IndexOf('`')
        if index <> -1 then
          let generateGenerics = 
            seq [ 1 .. (slice.Slice(index + 1).ToString() |> int) ]
            |> Seq.map (fun x -> "T" + x.ToString())

          String.Format("{0}<{1}>", slice.Slice(0, slice.IndexOf('`')).ToString(), TextHelpers.normal (String.Join(", ", generateGenerics)) tools) |> Normal
        else
          slice.ToString() |> Normal

    let memberReference cutter =
      let memberString: string = cutter()
      let typeString = reference.[..reference.Length - memberString.Length - 2]
      let typeRef = typeReference typeString

      let memberAnchor = 
        let mutable result: IType = null
        if tools.typeResolver.TryFindType(typeString.[2..], &result) then
          let mem = tryFindMember(result, reference, memberString)
          if Option.isSome mem then
            LinkContent (Normal memberString, tools.linker.CreateAnchor(input, mem |> Option.get))
          else
            memberString |> Normal
        else
          memberString |> Normal

      (seq [ typeRef; memberAnchor ], ".") |> JoinedText

    let cutMethod() = 
      reference.Substring(reference.AsSpan(0, reference.IndexOf('(')).LastIndexOf('.') + 1)
    let cutMember() = 
      reference.Substring(reference.LastIndexOf('.') + 1)

    match reference.[0] with
    | 'T' -> typeReference reference
    | 'E' -> typeReference reference
    | 'M' -> memberReference cutMethod
    | 'P' -> memberReference cutMember
    | 'F' -> memberReference cutMember
    | _ -> reference.Substring(2) |> Normal

  let processResType source (item: IResType) tools =
    let tryLink (item: IResType) =
      let link = tools.linker.CreateLink(source, item)
      if not (String.IsNullOrEmpty link) then
        (InlineCode item.DisplayName, lazy(link)) |> LinkContent
      else
        InlineCode item.DisplayName

    // TODO: Generic arrays?
    match item with
    | :? IResGeneric as generic ->
      let generics = generic.Generics
                     |> Seq.map tryLink
      let content = seq [ tryLink generic; "<" |> Normal; (generics, ", ") |> JoinedText; ">" |> Normal ]
      (content, "") |> JoinedText
    | _ -> tryLink item

  let methodArguments source (item: IMember) tools =
    let argument arg =
      JoinedText ([ arg |> StringConverters.argumentTypeStr |> Normal; processResType source arg.Type tools; Normal arg.Name ], " ")
    let processArguments args =
      JoinedText (args |> Seq.map argument, ", ")

    match item with 
    | :? IConstructor as c -> c.Arguments |> processArguments
    | :? IDelegate as d -> d.Arguments |> processArguments
    | _ -> raise (Exception())

  let rec processContent (input, item, tools): Element<ILink> option =
    let applyTools input =
      input tools

    let getInlineText (tag: IInnerTag) =
      tag.Content
      |> Seq.where(fun x -> x :? ITextTag)
      |> Seq.map (fun x -> (x :?> ITextTag).Content)

    let processColumn(column: seq<IContent>) =
      column
      |> Seq.map (fun x -> processContent(input, x, tools))
      |> SomeHelpers.whereSome
      |> Seq.map (ElementHelpers.initialize >> applyTools)

    match item with
    | :? ITextTag as text
      -> Some(text.Content |> Normal |> TextElement)
    | :? IInnerTag as inner ->
      match inner.Type with
      | IInnerTag.InnerTagType.CodeSingle
        -> Some(getInlineText inner |> Seq.exactlyOne |> InlineCode |> TextElement)
      | IInnerTag.InnerTagType.Code
        -> Some(getInlineText inner |> Seq.exactlyOne |> Code |> TextElement)
      | IInnerTag.InnerTagType.ParamRef
      | IInnerTag.InnerTagType.TypeRef
        -> Some(inner.Reference |> InlineCode |> TextElement)
      | IInnerTag.InnerTagType.See
      | IInnerTag.InnerTagType.SeeAlso
        -> Some(processReference(input, inner.Reference, tools) |> TextElement)
      | IInnerTag.InnerTagType.Para
        -> Some(Environment.NewLine |> Normal |> TextElement)
      | _ -> None
    | :? IListTag as list ->
      match list.Type with
      | IListTag.ListType.Table ->
        let content = list.Rows
                      |> Seq.map (processColumn >> Linq.ToReadOnlyCollection)
        let headings = list.Headings
                       |> Seq.map (fun x -> processContent(input, x, tools))
                       |> SomeHelpers.whereSome
                       |> Seq.filter ElementHelpers.isTextElement
                       |> Seq.map(fun x -> x |> (ElementHelpers.initialize >> applyTools) :?> IText)
        Element<ILink>.Table(content, headings, "", 0) |> Some
      | _ ->
        let listType (t: IListTag.ListType) =
          match t with
          | IListTag.ListType.Bullet -> IList.ListType.Dotted
          | IListTag.ListType.Number -> IList.ListType.Numbered
          | _ -> raise (Exception()) // TODO: Message

        let content = list.Rows
                      |> Seq.collect id
                      |> Seq.map (fun x -> processContent(input, x, tools))
                      |> SomeHelpers.whereSome
                      |> Seq.map (ElementHelpers.initialize >> applyTools)
        Element<ILink>.ListElement(content, listType list.Type, "", 0) |> Some
    | _ -> None

  let tagShort (input, tag: ITag, tools) =
    let getCount =
      let isInvalid (item: IContent) =
        match item with
        | :? IListTag -> true
        | :? IInnerTag as tag ->
          match tag.Type with
          | IInnerTag.InnerTagType.See
          | IInnerTag.InnerTagType.SeeAlso
          | IInnerTag.InnerTagType.Code
          | IInnerTag.InnerTagType.InvalidTag -> true
          | _ -> false
        | _ -> false

      match (tag.Content |> Seq.tryFindIndex isInvalid) with
      | None -> tag.Content.Count
      | Some x -> x

    let count = getCount
    let readMore = if (count <> tag.Content.Count) then Some("..." |> Normal |> TextElement) else None
    let processed = tag.Content
                    |> Seq.take count
                    |> Seq.map (fun x -> processContent(input, x, tools))
    let content = seq [readMore]
                  |> Seq.append processed
                  |> SomeHelpers.whereSome
                  |> ElementHelpers.getTextContent

    JoinedText (content, " ")

  let tagFull (input: IType, tag: ITag, tools) =
    let content = tag.Content
                  |> Seq.map (fun x -> processContent(input, x, tools))
                  |> SomeHelpers.whereSome
                  |> Seq.map (fun x -> ElementHelpers.initialize x tools)

    let list = new LinkedList<ITextContent>()
    let result = seq [
      for item in content do
        if (item :? ITextContent && (not (item :? IText) || (item :?> IText).Style <> IText.TextStyle.Code)) then
          list.AddLast (item :?> ITextContent) |> ignore
        elif (list.Count = 0) then
          yield item
        else
          let joined = tools.creator.JoinTextContent(list, " ") |> ElementHelpers.toElement
          list.Clear()

          yield joined
          yield item
    ]

    if (Seq.isEmpty result) then
      seq [ tools.creator.JoinTextContent(list, " ") |> ElementHelpers.toElement ]
    else
      result

  let memberNameSummary(input: IType, name: TextType<ILink>, summary: Option<ITag>, tools) =
    match summary with
    | None -> name
    | Some x -> JoinedText (seq [ name; tagShort(input, x, tools) ], Environment.NewLine)

  let findTypeTag(input: IType, tag: ITag.TagType, tools) =
    seq [
      let mutable typeDoc: IDocElement = null
      if (tools.docResolver.TryFindType(input, &typeDoc)) then
        let mutable result: IReadOnlyCollection<ITag> = null
        if (typeDoc.Documentation.Tags.TryGetValue(tag, &result)) then
          result
    ]
    |> Seq.collect id

  let findTag(input: IType, mem: IMember, tag: ITag.TagType, tools) =
    seq [
      let mutable typeDoc: IDocElement = null
      if (tools.docResolver.TryFindType(input, &typeDoc)) then
        let mutable memberDoc: IDocMember = null
        if (typeDoc.Members.Value.TryGetValue(mem.RawName, &memberDoc)) then
          let mutable result: IReadOnlyCollection<ITag> = null
          if (memberDoc.Documentation.Tags.TryGetValue(tag, &result)) then
            result
    ]
    |> Seq.collect id

  let registerSection(input, name, content, tools, level) =
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

      let inter = content |> Seq.map processContent

      composeSections inter 4
      |> Seq.map (applyTools >> ElementHelpers.toElement)
      |> Seq.append (seq [ signature |> applyMember |> TextHelpers.processText |> applyTools |> ElementHelpers.toElement ])
    let single x =
      let tag = findTypeTag(input, x, tools) |> Seq.tryExactlyOne
      if Option.isNone tag then
        None
      else
        tagFull(input, tag |> Option.get, tools) |> SomeHelpers.emptyToNone
    let genericProcessor provider processItem =
      let processItems (items: 'M IReadOnlyCollection when 'M :> IMember) =
        items
        |> Seq.mapi (fun x y -> processItem(x, y) |> registerSection |> ElementHelpers.toElement)

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
          ListElement (group |> Seq.map (fun x -> ElementHelpers.initialize (getTypeName x |> InlineCode |> TextElement) tools), IList.ListType.Dotted, heading, 3)
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
        |> Seq.map (fun x -> ElementHelpers.initialize (processResType input x tools |> TextElement) tools)

      let createList l =
        if (Seq.isEmpty l) then
          None
        else
          seq [ tools.creator.CreateList(l, IList.ListType.Dotted) |> ElementHelpers.toElement ] |> Some

      match input with
      | :? IClass as x ->
        let baseType = if (isNull x.BaseClass) then None else ElementHelpers.initialize (processResType input x.BaseClass tools |> TextElement) tools |> Some
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
                          |> Seq.map (fun x -> processResType input x tools)
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
            yield tagShort(input, x, tools)

            if (Option.isSome constraints) then
              yield constraints |> Option.get
          ]
          |> Seq.map (TextHelpers.processText >> applyTools >> ElementHelpers.toElement)

        if input :? IInterface then
          findTypeTag(input, ITag.TagType.Typeparam, tools)
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
        | Some tag -> tagFull(input, tag, tools) |> Seq.map ElementHelpers.toElement |> SomeHelpers.emptyToNone
        | _ -> None
      findTag(input, m, t, tools)
      |> Seq.tryExactlyOne
      |> single
    let getExceptions m =
      let exceptions = findTag(input, m, ITag.TagType.Exception, tools)
                       |> Seq.map (fun x -> seq [ processReference(input, x.Reference, tools); tagShort(input, x, tools) ] |> Seq.map (TextHelpers.processText >> applyTools >> ElementHelpers.toElement) |> Linq.ToReadOnlyCollection)

      if Seq.isEmpty exceptions then
        None
      else
        seq [ tools.creator.CreateTable(exceptions, createHeadings (seq [ "Name"; "Description" ]) tools) |> ElementHelpers.toElement ] |> Some
    let getSeeAlso m =
      let seeAlsos = findTag(input, m, ITag.TagType.Seealso, tools)
                     |> Seq.map (fun x -> tagShort(input, x, tools) |> TextHelpers.processText |> applyTools |> ElementHelpers.toElement)

      if Seq.isEmpty seeAlsos then
        None
      else
        seq [ tools.creator.CreateList(seeAlsos, IList.ListType.Dotted) |> ElementHelpers.toElement ] |> Some
    let getArguments (c: IMember) = 
      let argumentDocs = findTag(input, c, ITag.TagType.Param, tools)
                         |> Seq.map (fun x -> x.Reference, tagShort(input, x, tools) |> TextHelpers.processText |> applyTools |> ElementHelpers.toElement)
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

          JoinedText (seq [ processResType input x.Type tools ] |> Seq.append argType, " ")

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
          (getTypeName t |> Normal, tools.linker.CreateLink(input, t)) |> LinkElement

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
            (getArguments , "Arguments")
            (getSingleTag ITag.TagType.Summary, "Summary")
            (getSingleTag ITag.TagType.Remarks, "Remarks")
            (getSingleTag ITag.TagType.Example, "Example")
            (getExceptions, "Exceptions")
            (getSeeAlso, "See also")
          ]

        (ctor, ctor.Name + overloadFormat extractor.Count i, joinContentWSig content signature ctor, tools, 3)

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

        (method, (if method.Operator <> OperatorType.None then "Operator " else "") + method.Name + getOverloads, joinContentWSig content signature method, tools, 3)

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

        (property, property.Name, joinContentWSig content signature property, tools, 3)

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

        (event, event.Name, joinContentWSig content signature event, tools, 3)

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

        (deleg, deleg.Name + getOverloads, joinContentWSig content signature deleg, tools, 3)

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

        (field, field.Name, (if Seq.isEmpty content then seq [ TextHelpers.empty tools |> ElementHelpers.toElement ] else content), tools, 3)

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

