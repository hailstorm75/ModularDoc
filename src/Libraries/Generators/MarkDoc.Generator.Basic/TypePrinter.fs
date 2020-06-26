namespace MarkDoc.Generator.Basic

open System
open Helpers
open MarkDoc.Documentation.Tags
open MarkDoc.Members.ResolvedTypes
open MarkDoc.Members.Members
open MarkDoc.Members.Types
open MarkDoc.Members
open MarkDoc.Documentation
open MarkDoc.Generator
open MarkDoc.Elements
open MarkDoc.Linkers
open MarkDoc.Helpers
open System.Collections.Generic

type TypePrinter(creator, resolver, linker) =
  let m_creator  : IElementCreator = creator
  let m_resolver : IDocResolver    = resolver
  let m_linker   : ILinker         = linker

  let textNormal x = m_creator.CreateText(x, IText.TextStyle.Normal)
  let textBold   x = m_creator.CreateText(x, IText.TextStyle.Bold)
  let textItalic x = m_creator.CreateText(x, IText.TextStyle.Italic)
  let textCode   x = m_creator.CreateText(x, IText.TextStyle.Code)
  let textInline x = m_creator.CreateText(x, IText.TextStyle.CodeInline)

  let createHeadings headings =
    headings |> Seq.map textNormal

  let getTypeName (input : IType) =
    let joinGenerics (i : seq<string>) =
      let generics = i |> partial String.Join ", "
      "<" + generics + ">"
    let processInterface (input : IInterface) =
      let generics =
        input.Generics
        |> Seq.map (fun x -> (x.Value.ToTuple() |> fst |> varianceStr) + " " + x.Key)
      if not (Seq.isEmpty generics) then
        input.Name + joinGenerics generics
      else
        input.Name
    let processStruct (input : 'M when 'M :> IInterface) =
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

  let processResType (item : IResType) =
    let tryLink (item : IResType) =
      let link = m_linker.CreateLink(item)
      if not (String.IsNullOrEmpty link) then
        m_creator.CreateLink(textInline item.DisplayName, link) :> ITextContent
      else
        textInline item.DisplayName :> ITextContent

    // TODO: Generic arrays?
    match item with
    | :? IResGeneric as generic ->
      let generics = generic.Generics
                     |> Seq.map tryLink
      let content = seq [ tryLink generic; textNormal "<" :> ITextContent; m_creator.JoinTextContent(generics, ", "); textNormal ">" :> ITextContent ]
      m_creator.JoinTextContent(content, "")
    | _ -> tryLink item

  let processResType2(item : IResType) =
    let tryLink (item : IResType) =
      item.DisplayName

    // TODO: Generic arrays?
    match item with
    | :? IResGeneric as generic ->
      let generics = generic.Generics
                     |> Seq.map tryLink
      let content = seq [ tryLink generic; "<"; String.Join(", ", generics); ">" ]
      String.Join("", content)
    | _ -> tryLink item

  // TODO: Refactor
  let methodArguments (item : IConstructor) =
    let argument (arg : IArgument) =
      let args = seq [ arg |> (argumentTypeStr >> textNormal) :> ITextContent; processResType arg.Type; textNormal arg.Name :> ITextContent ]
      m_creator.JoinTextContent(args, " ")

    m_creator.JoinTextContent(item.Arguments |> Seq.map argument, ", ")

  // TODO: Refactor
  let methodArguments2(item : IConstructor) =
    let argument (arg : IArgument) =
      let args =
        seq [ arg |> argumentTypeStr; processResType2 arg.Type; arg.Name ]
        |> Seq.filter (String.IsNullOrEmpty >> not)
      String.Join(" ", args)

    String.Join(", ", item.Arguments |> Seq.map argument)

  let rec processContent (item : IContent) =
    let getInlineText (tag : IInnerTag) =
      tag.Content
      |> Seq.where(fun x -> x :? ITextTag)
      |> Seq.map (fun x -> (x :?> ITextTag).Content)

    let processColumn(column : seq<IContent>) =
      column
      |> Seq.map processContent
      |> whereSome
      |> Seq.map toElement

    match item with
    | :? ITextTag as text -> Some(textNormal text.Content |> toElement)
    | :? IInnerTag as inner ->
      match inner.Type with
      | IInnerTag.InnerTagType.CodeSingle
        -> Some(getInlineText inner |> Seq.exactlyOne |> textInline |> toElement)
      | IInnerTag.InnerTagType.Code
        -> Some(getInlineText inner |> Seq.exactlyOne |> textCode |> toElement)
      | IInnerTag.InnerTagType.ParamRef
      | IInnerTag.InnerTagType.TypeRef
        -> Some(textInline inner.Reference |> toElement)
      | IInnerTag.InnerTagType.See
      | IInnerTag.InnerTagType.SeeAlso
        -> Some(textBold inner.Reference |> toElement) // TODO: Create link
      | IInnerTag.InnerTagType.Para
        -> Some(textNormal Environment.NewLine |> toElement)
      | _ -> None
    | :? IListTag as list ->
      match list.Type with
      | IListTag.ListType.Table ->
        let content = list.Rows
                      |> Seq.map (processColumn >> Linq.ToReadOnlyCollection)
        let headings = list.Headings
                       |> Seq.map processContent
                       |> whereSome
                       |> Seq.filter(fun x-> x :? IText)
                       |> Seq.map(fun x -> x :?> IText)
        Some(m_creator.CreateTable(content, headings) |> toElement)
      | _ ->
        let content = list.Rows
                      |> Seq.collect id
                      |> Seq.map processContent
                      |> whereSome

        Some(m_creator.CreateList(content, listType list.Type) |> toElement)
    | _ -> None

  let tagShort (x : ITag) =
    let getCount =
      let isInvalid (item : IContent) =
        match item with
        | :? IListTag -> true
        | :? IInnerTag as tag ->
          match tag.Type with
          | IInnerTag.InnerTagType.Code
          | IInnerTag.InnerTagType.InvalidTag -> true
          | _ -> false
        | _ -> false

      match (x.Content |> Seq.tryFindIndex isInvalid) with
      | None -> x.Content.Count
      | Some x -> x

    let count = getCount
    let readMore = if (count <> x.Content.Count) then Some(textNormal "..." |> toElement) else None
    let processed = x.Content
                    |> Seq.take count
                    |> Seq.map processContent
    let content = seq [readMore]
                  |> Seq.append processed
                  |> whereSome
                  |> Seq.filter(fun x -> x :? ITextContent)
                  |> Seq.map(fun x -> x :?> ITextContent)

    m_creator.JoinTextContent(content, " ")

  let tagFull (x : ITag) =
    let content = x.Content
                  |> Seq.map processContent
                  |> whereSome

    let list = new LinkedList<ITextContent>()
    let result = seq [
      for item in content do
      if (item :? ITextContent) then
        list.AddLast (item :?> ITextContent) |> ignore
      elif (list.Count = 0) then
        yield item
      else
        let joined = m_creator.JoinTextContent(list, " ") |> toElement
        list.Clear()

        yield joined
        yield item
    ]

    if (Seq.isEmpty result) then
      list |> Seq.map toElement
    else
      result

  let memberNameSummary(name : ITextContent, summary : Option<ITag>) =
    match summary with
    | None -> name
    | Some x -> m_creator.JoinTextContent(seq [ name; tagShort x ], Environment.NewLine)

  let findTypeTag(input : IType, tag : ITag.TagType) =
    seq [
      let mutable typeDoc : IDocElement = null
      if (m_resolver.TryFindType(input, &typeDoc)) then
        let mutable result : IReadOnlyCollection<ITag> = null
        if (typeDoc.Documentation.Tags.TryGetValue(tag, &result)) then
          result
    ]
    |> Seq.collect id

  let findTag(input : IType, mem : IMember, tag : ITag.TagType) =
    seq [
      let mutable typeDoc : IDocElement = null
      if (m_resolver.TryFindType(input, &typeDoc)) then
        let mutable memberDoc : IDocMember = null
        if (typeDoc.Members.Value.TryGetValue(mem.RawName, &memberDoc)) then
          let mutable result : IReadOnlyCollection<ITag> = null
          if (memberDoc.Documentation.Tags.TryGetValue(tag, &result)) then
            result
    ]
    |> Seq.collect id

  let printIntroduction(input : IType) =
    match findTypeTag(input, ITag.TagType.Summary) |> Seq.tryExactlyOne with
    | None -> None
    | Some x -> Some(seq [ x |> tagShort :> IElement])

  let printMemberTables(input : IType) =
    let processStruct(input : IInterface) =
      let sectionHeading isStatic accessor section =
        seq [ accessorStr accessor; staticStr isStatic; section ]
        |> partial String.Join " "

      let createContent (members : seq<'M> when 'M :> IMember, newRow) =
        members
        |> Seq.groupBy (fun x-> x.Name)
        |> Seq.sortBy fst
        |> Seq.collect snd
        |> Seq.map newRow

      let createPropertySection(isStatic, accessor, properties : seq<IProperty>) =
        let createRow(property : IProperty) =
          let processName =
            let name = textInline property.Name :> ITextContent
            memberNameSummary(name, findTag(input, property, ITag.TagType.Summary) |> Seq.tryExactlyOne)

          let processMethods =
            let getter =
              if property.GetAccessor.HasValue then
                seq [ textInline "get" :> ITextContent ]
              else
                Seq.empty
            let setter =
              if property.SetAccessor.HasValue then
                seq [ textInline "set" :> ITextContent ]
              else
                Seq.empty

            m_creator.JoinTextContent(Seq.append getter setter, " ")

          seq [ processResType property.Type; processName; processMethods ]
          |> Seq.map toElement
          |> Linq.ToReadOnlyCollection

        let grouped = createContent(properties, createRow)

        if (Seq.isEmpty properties) then
          None
        else
          m_creator.CreateTable(grouped, [ "Type"; "Name"; "Methods" ] |> createHeadings, sectionHeading isStatic accessor "properties", 3)
          :> IElement
          |> Some

      let createMethodSection(isStatic, accessor, methods : seq<IMethod>) =
        let methodsArray = methods |> Seq.toArray

        let createRow(method : IMethod) =
          let processReturn =
            let name = if isNull method.Returns then "void" else method.Returns.DisplayName
            let content = textInline name
            if isNull method.Returns then
              content :> ITextContent
            else
              let link = m_linker.CreateLink method.Returns
              if String.IsNullOrEmpty link then
                content :> ITextContent
              else
                m_creator.CreateLink(content, link) :> ITextContent

          let processMethod =
            let hasOverloads =
              methodsArray
              |> Seq.where(fun x -> x.Name = method.Name)
              |> Seq.skip 1
              |> Seq.isEmpty
              |> not

            let signature = seq [ textInline method.Name :> ITextContent; textNormal "(" :> ITextContent;  (if hasOverloads then textInline "..." :> ITextContent else (methodArguments method)); textNormal ")" :> ITextContent; ]
            let signatureText = m_creator.JoinTextContent(signature, "")
            memberNameSummary(signatureText, findTag(input, method, ITag.TagType.Summary) |> Seq.tryExactlyOne)

          seq [ processReturn; processMethod ]
          |> Seq.map toElement
          |> Linq.ToReadOnlyCollection

        let grouped = createContent(methodsArray |> Seq.distinctBy(fun x -> x.Name), createRow)

        if (Seq.isEmpty methods) then
          None
        else
          m_creator.CreateTable(grouped, [ "Returns"; "Name" ] |> createHeadings, sectionHeading isStatic accessor "methods", 3)
          :> IElement
          |> Some

      let processMembers item =
        item
        |> Seq.map flatten
        |> Seq.collect id

      let createTable x f =
        x
        |> groupMembers
        |> processMembers
        |> Seq.map f
        |> Seq.filter Option.isSome

      seq [
        (createTable input.Properties createPropertySection, "Properties");
        (createTable input.Methods createMethodSection, "Methods");
      ]
      |> Seq.filter (fst >> Seq.isEmpty >> not)
      |> Seq.map(fun x -> m_creator.CreateSection(x |> fst |> Seq.map Option.get, snd x, 2) |> toElement)

    match input with
    | :? IInterface as x -> processStruct x
    | _ -> Seq.empty
    |> emptyToNone

  let printDetailed(input : IType) =
    let single x =
      let tag = findTypeTag(input, x) |> Seq.tryExactlyOne
      if Option.isNone tag then
        None
      else
        tag |> Option.get |> tagFull |> Some

    let nested =
      let getNested (x : IStruct) =
        x.NestedTypes

      let groupByType (x : IType) =
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

      let processGroup (x : string option * seq<IType>) =
        let createTable (heading : string, group : seq<IType>) =
          m_creator.CreateList(group |> Seq.map (getTypeName >> textInline >> toElement), IList.ListType.Dotted, heading, 3)
          |> toElement
          |> Some

        match x |> (fst >> Option.get) with
        | "c" -> createTable("Classes", snd x)
        | "i" -> createTable("Interfaces", snd x)
        | "s" -> createTable("Structures", snd x)
        | "e" -> createTable("Enums", snd x)
        | _ -> None

      match input with
      | :? IStruct as x ->
           x
           |> getNested
           |> Seq.groupBy groupByType
           |> Seq.filter (fst >> Option.isSome)
           |> Seq.map processGroup
           |> whereSome
           |> emptyToNone
      | _ -> None

    let inheritance =
      let getInterfaces (x : 'M when 'M :> IInterface) =
        x.InheritedInterfaces
        |> Seq.map (processResType >> toElement)

      let createList l =
        if (Seq.isEmpty l) then
          None
        else
          seq [ m_creator.CreateList(l, IList.ListType.Dotted) |> toElement ] |> Some

      match input with
      | :? IClass as x ->
        let baseType = if (isNull x.BaseClass) then None else x.BaseClass |> processResType |> toElement |> Some
        let interfaces = getInterfaces x
        seq [ baseType ]
        |> whereSome
        |> Seq.append interfaces
        |> createList
      | :? IInterface as x ->
        x |> (getInterfaces >> createList)
      | _ -> None

    let typeParams =
      let getTypeParams =
        let processTag (x : ITag) =
          let generics = (input :?> IInterface).Generics
          let getConstraints (x : ITag) =
            if generics.ContainsKey(x.Reference) then
              let types = generics.[x.Reference].ToTuple()
                          |> snd
                          |> Seq.map processResType
              m_creator.JoinTextContent(types, Environment.NewLine) |> Some
            else
              None

          let getName (x : ITag) =
            let result = seq [
              yield textInline x.Reference :> ITextContent
              if (generics.ContainsKey(x.Reference)) then
                let variance = generics.[x.Reference].ToTuple() |> fst
                if (variance <> Enums.Variance.NonVariant) then
                  yield variance |> varianceStr |> textInline :> ITextContent
            ]
            m_creator.JoinTextContent(result, " ") |> toElement

          let constraints = getConstraints x
          seq [
            yield getName x
            yield tagShort x |> toElement

            if (Option.isSome constraints) then
              yield constraints |> Option.get |> toElement
          ]

        if input :? IInterface then
          findTypeTag(input, ITag.TagType.Typeparam)
          |> Seq.map (processTag >> Linq.ToReadOnlyCollection)
          |> Some
        else
          None

      let ts = getTypeParams
      if (Option.isSome ts && ts |> (Option.get >> Seq.isEmpty >> not)) then
        seq [ m_creator.CreateTable(ts |> Option.get, seq [ "Type"; "Description"; "Constraints" ] |> createHeadings) |> toElement ] |> Some
      else
        None

    let constructors =
      let processCtors (ctors : IReadOnlyCollection<IConstructor>) =
        let processCtor (i : int, ctor : IConstructor) =
          let single (tags : ITag option) =
            match tags with
            | Some -> tags |> Option.get |> tagFull |> Some
            | None -> None

          let overloads =
            if ctors.Count > 1 then
              String.Format(" [{0}/{1}]", i + 1, ctors.Count)
            else
              ""
          let toLower(x : string) =
            x.ToLower()
          let signature =
            (ctor.Accessor |> accessorStr |> toLower) + (if ctor.IsStatic then " static " else " ") + ctor.Name + "(" + (methodArguments2 ctor) + ")"
            |> textCode
            |> toElement
          let summary = findTag(input, ctor, ITag.TagType.Summary)
                        |> Seq.tryExactlyOne
                        |> single
          let remarks = findTag(input, ctor, ITag.TagType.Remarks)
                        |> Seq.tryExactlyOne
                        |> single
          let example = findTag(input, ctor, ITag.TagType.Example)
                        |> Seq.tryExactlyOne
                        |> single

          let content =
            seq [
              (summary, "Summary")
              (remarks, "Remarks")
              (example, "Example")
            ]
            |> Seq.filter (fst >> Option.isSome)
            |> Seq.map(fun x -> m_creator.CreateSection(fst x |> Option.get, snd x, 4) |> toElement)

          let joined = seq [ signature ]
                       |> Seq.append content

          m_creator.CreateSection(joined, ctor.Name + overloads, 3) |> toElement

        ctors
        |> Seq.mapi (fun x y -> processCtor(x, y))

      match input with
      | :? IClass as x -> x.Constructors |> processCtors |> emptyToNone
      | _ -> None

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
      ]
      |> Seq.filter (fst >> Option.isSome)
      |> Seq.map(fun x -> m_creator.CreateSection(fst x |> Option.get, snd x, 2) |> toElement)

    sections |> emptyToNone

  let printContent (input : IType) =
    let createSection(x : seq<IElement>, y : string) =
      m_creator.CreateSection(x, y, 1)

    seq [
      (printIntroduction input, "Description");
      (printMemberTables input, "Members");
      (printDetailed input, "Details")
    ]
    |> Seq.filter (fst >> Option.isSome)
    |> Seq.map (fun x -> (x |> fst |> Option.get, x |> snd))
    |> Seq.map (createSection >> toElement)

  interface ITypePrinter with
    member __.Print(input : IType) =
      if (isNull input) then
        raise (ArgumentNullException("input"))
      else
        m_creator.CreatePage(null, printContent input, getTypeName input)
