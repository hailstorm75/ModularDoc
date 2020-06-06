namespace MarkDoc.Generator.Basic

open System;
open MarkDoc.Generator;
open MarkDoc.Members;
open MarkDoc.Members.Enums;
open MarkDoc.Elements;
open MarkDoc.Documentation;
open MarkDoc.Linkers;
open MarkDoc.Helpers
open MarkDoc.Documentation.Tags
open System.Collections.Generic
open Helpers

type TypePrinter(creator, resolver, linker) =
  let m_creator  : IElementCreator = creator
  let m_resolver : IDocResolver    = resolver
  let m_linker   : ILinker         = linker

  let textNormal x = m_creator.CreateText(x, IText.TextStyle.Normal)
  let textBold   x = m_creator.CreateText(x, IText.TextStyle.Bold)
  let textItalic x = m_creator.CreateText(x, IText.TextStyle.Italic)
  let textCode   x = m_creator.CreateText(x, IText.TextStyle.Code)
  let textInline x = m_creator.CreateText(x, IText.TextStyle.CodeInline)

  let rec processContent (item : IContent) =
    let getInlineText (tag : IInnerTag) =
      tag.Content
      |> Seq.where(fun x -> x :? ITextTag)
      |> Seq.map (fun x -> (x :?> ITextTag).Content)

    let processColumn(column : seq<IContent>) =
      column
      |> Seq.map processContent
      |> Seq.filter Option.isSome
      |> Seq.map (Option.get >> toElement)

    match item with
    | :? ITextTag as text -> Some(textNormal text.Content |> toElement)
    | :? IInnerTag as inner ->
      match inner.Type with
      | IInnerTag.InnerTagType.CodeSingle -> Some(getInlineText inner |> Seq.exactlyOne |> textInline |> toElement)
      | IInnerTag.InnerTagType.Code -> Some(getInlineText inner |> Seq.exactlyOne |> textCode |> toElement)
      | IInnerTag.InnerTagType.ParamRef
      | IInnerTag.InnerTagType.TypeRef -> Some(textInline inner.Reference |> toElement)
      | IInnerTag.InnerTagType.See
      | IInnerTag.InnerTagType.SeeAlso -> Some(textBold inner.Reference |> toElement) // TODO: Create link
      | IInnerTag.InnerTagType.Para -> Some(textNormal Environment.NewLine |> toElement)
      | _ -> None
    | :? IListTag as list ->
      match list.Type with
      | IListTag.ListType.Table ->
        let content = list.Rows
                      |> Seq.map (processColumn >> Linq.ToReadOnlyCollection)
        let headings = list.Headings
                       |> Seq.map processContent
                       |> Seq.filter Option.isSome
                       |> Seq.map Option.get
                       |> Seq.filter(fun x-> x :? IText)
                       |> Seq.map(fun x -> x :?> IText)
        Some(m_creator.CreateTable(content, headings) |> toElement)
      | _ ->
        let content = list.Rows
                      |> Seq.collect id
                      |> processColumn
        Some(m_creator.CreateList(content, listType list.Type) |> toElement)
    | _ -> None

  let tagShort (x : ITag) =
    let getCount =
      let isInvalid (item : IContent) =
        match item with
        | :? IListTag -> true
        | :? IInnerTag as tag ->
          match tag.Type with
          | IInnerTag.InnerTagType.Para
          | IInnerTag.InnerTagType.Code
          | IInnerTag.InnerTagType.InvalidTag -> true
          | _ -> false
        | _ -> false

      match (x.Content |> Seq.tryFindIndex isInvalid) with
      | None -> x.Content.Count
      | Some x -> x

    let count = getCount
    let readMore = if (count <> x.Content.Count) then Some(textNormal "..." |> toElement) else None
    let content = x.Content
                  |> Seq.take count
                  |> Seq.map processContent
                  |> Seq.append(seq [readMore])
                  |> Seq.filter Option.isSome
                  |> Seq.map Option.get
                  |> Seq.filter(fun x -> x :? ITextContent)
                  |> Seq.map(fun x -> x :?> ITextContent)

    m_creator.JoinTextContent(content, " ")

  let tagFull (x : ITag) =
    x.Content
    |> Seq.map processContent
    |> Seq.filter Option.isSome
    |> Seq.map Option.get
    
  let memberNameSummary(name : ITextContent, summary : Option<ITag>) =
    match summary with
    | None -> name
    | Some x -> m_creator.JoinTextContent(seq [ name; tagShort x ], "<br>")

  let findTypeTag(input : IType, tag : ITag.TagType) =
    let mutable typeDoc : IDocElement = null
    if not (m_resolver.TryFindType(input, &typeDoc)) then
      Seq.empty
    else
      let mutable result : IReadOnlyCollection<ITag> = null
      if not (typeDoc.Documentation.Tags.TryGetValue(tag, &result)) then
        Seq.empty
      else
        result |> Seq.cast

  let findTag(input : IType, mem : IMember, tag : ITag.TagType) =
    let mutable typeDoc : IDocElement = null
    if not (m_resolver.TryFindType(input, &typeDoc)) then
      Seq.empty
    else
      let mutable memberDoc : IDocMember = null
      if not (typeDoc.Members.Value.TryGetValue(mem.RawName, &memberDoc)) then
        Seq.empty
      else
        let mutable result : IReadOnlyCollection<ITag> = null
        if not (memberDoc.Documentation.Tags.TryGetValue(tag, &result)) then
          Seq.empty
        else
          result |> Seq.cast

  let printIntroduction(input : IType) =
    match findTypeTag(input, ITag.TagType.Summary) |> Seq.tryExactlyOne with
    | None -> None
    | Some x -> Some(seq [x |> tagShort :> IElement])

  let printMemberTables(input : IType) =
    let processInterface(input : IInterface) =
      let createHeadings headings =
        headings |> Seq.map textNormal
      let sectionHeading isStatic accessor section =
        seq [ accessorStr accessor; staticStr isStatic; section ]
        |> partial String.Join " "

      let createContent (members : seq<'M> when 'M :> IMember, newRow) =
        members
        |> Seq.groupBy (fun x-> x.Name)
        |> Seq.sortBy fst
        |> Seq.collect snd
        |> Seq.map newRow

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

            let signature = method.Name + "(" + (if hasOverloads then "..." else (methodArguments method)) + ")"
            let signatureText = textInline signature
            memberNameSummary(signatureText, findTag(input, method, ITag.TagType.Summary) |> Seq.tryExactlyOne)

          seq [ processReturn; processMethod ]
          |> Seq.map toElement
          |> Linq.ToReadOnlyCollection

        let grouped = createContent(methodsArray, createRow)

        m_creator.CreateTable(grouped, [ "Returns"; "Name" ] |> createHeadings, sectionHeading isStatic accessor "methods", 3)

      let createPropertySection(isStatic, accessor, properties : seq<IProperty>) =
        let createRow(property : IProperty) =
          let processType =
            let content = textInline property.Type.DisplayName
            let link = m_linker.CreateLink(property.Type)
            if (String.IsNullOrEmpty link) then
              content :> ITextContent
            else
              m_creator.CreateLink(content, link) :> ITextContent

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

          seq [ processType; processName; processMethods ]
          |> Seq.map toElement
          |> Linq.ToReadOnlyCollection

        let grouped = createContent(properties, createRow)

        m_creator.CreateTable(grouped, [ "Type"; "Name"; "Methods" ] |> createHeadings, sectionHeading isStatic accessor "properties", 3)

      let processMembers item =
        item
        |> Seq.map flatten
        |> Seq.collect id

      let createTable x f =
        x
        |> groupMembers
        |> processMembers
        |> Seq.map (f >> toElement)

      let methods = createTable input.Methods createMethodSection
      let properties = createTable input.Properties createPropertySection

      seq [
        m_creator.CreateSection(methods, "Methods", 2) :> IElement;
        m_creator.CreateSection(properties, "Properties", 2) :> IElement;
      ]

    match input with
    | :? IInterface as x -> Some(processInterface x)
    | _ -> None

  let printDetailed(input : IType) =
    let single x =
      let tag = findTypeTag(input, x) |> Seq.tryExactlyOne
      if Option.isNone tag then
        None
      else
        Some(tag |> Option.get |> tagFull)

    let singles =
      seq [
        (single ITag.TagType.Summary, "Summary");
        (single ITag.TagType.Remarks, "Remarks");
        (single ITag.TagType.Example, "Example")
      ]
      |> Seq.filter (fst >> Option.isSome)
      |> Seq.map(fun x -> m_creator.CreateSection(fst x |> Option.get, snd x, 2) |> toElement)

    Some(singles)

  let printContent (input : IType) =
    let createSection(x : seq<IElement>, y : string)=
      m_creator.CreateSection(x, y, 1)

    seq [
      (printIntroduction input, "Description");
      (printMemberTables input, "Members");
      (printDetailed input, "Detailed description")
    ]
    |> Seq.filter (fst >> Option.isSome)
    |> Seq.map (fun x -> (x |> fst |> Option.get, x |> snd))
    |> Seq.map (createSection >> toElement)

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
    let processClass (input : IClass) =
      let generics =
        input.Generics
        |> Seq.map (fun x -> x.Key)
      if not (Seq.isEmpty generics) then
        input.Name + joinGenerics generics
      else
        input.Name

    match input with
    | :? IClass as x -> processClass x
    | :? IInterface as x -> processInterface x
    | _ -> input.Name

  interface ITypePrinter with
    member __.Print(input : IType) =
      if (isNull input) then
        raise (ArgumentNullException("input"))
      else
        m_creator.CreatePage(null, printContent input, getTypeName input)
