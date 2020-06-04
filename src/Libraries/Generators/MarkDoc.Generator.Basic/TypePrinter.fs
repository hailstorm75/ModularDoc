﻿namespace MarkDoc.Generator.Basic

open MarkDoc.Generator;
open MarkDoc.Members;
open MarkDoc.Members.Enums;
open MarkDoc.Elements;
open MarkDoc.Documentation;
open MarkDoc.Linkers;
open MarkDoc.Helpers
open MarkDoc.Documentation.Tags
open System.Collections.Generic

type TypePrinter(creator, resolver, linker) = 
  let m_creator  : IElementCreator = creator
  let m_resolver : IDocResolver    = resolver
  let m_linker   : ILinker         = linker

  let toElement x = x :> IElement 
  let textNormal x = m_creator.CreateText(x, IText.TextStyle.Normal)
  let textBold x = m_creator.CreateText(x, IText.TextStyle.Bold)
  let textItalic x = m_creator.CreateText(x, IText.TextStyle.Italic)
  let textCode x = m_creator.CreateText(x, IText.TextStyle.Code)
  let textInline x = m_creator.CreateText(x, IText.TextStyle.CodeInline)

  let summaryShort (x : ITag) =
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

    let processContent (item : IContent) = 
      match item with
      | :? ITextTag as text -> Some(textNormal text.Content :> ITextContent)
      | :? IInnerTag as inner ->
        match inner.Type with
        //| IInnerTag.InnerTagType.CodeSingle -> textInline inner.Content |> Seq.exactlyOne
        | IInnerTag.InnerTagType.ParamRef
        | IInnerTag.InnerTagType.TypeRef -> Some(textInline inner.Reference :> ITextContent)
        | IInnerTag.InnerTagType.See
        | IInnerTag.InnerTagType.SeeAlso -> Some(textBold inner.Reference :> ITextContent) // TODO: Create link
        | _ -> None
      | _ -> None

    let content = x.Content
                  |> Seq.take getCount
                  |> Seq.map processContent
                  |> Seq.filter(fun x -> x <> None)
                  |> Seq.map(fun x -> x.Value)
    m_creator.JoinTextContent(content, " ")

  let memberNameSummary(name : ITextContent, summary : Option<ITag>) =
    match summary with
    | None -> name
    | Some x -> m_creator.JoinTextContent(seq [ name; summaryShort x ], "<br>")

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
          result |> Seq.map(fun x -> x)

  let printMemberTables(input : IInterface) =
    let createHeadings headings =
      headings
      |> Seq.map textNormal

    let groupMembers (members : seq<'M> when 'M :> IMember) =
      let byStatic(x : 'M) =
        not x.IsStatic
      let byAccessor (x : bool * seq<'M>) =
        (fst x, snd x |> Seq.groupBy(fun y -> y.Accessor))

      members
      |> Seq.groupBy byStatic
      |> Seq.map byAccessor

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
            if System.String.IsNullOrEmpty link then
              content :> ITextContent
            else
              m_creator.CreateLink(content, link) :> ITextContent

        let processMethod = 
          let hasOverloads =
            methodsArray
            |> Seq.where(fun x -> x.Name = method.Name)
            |> Seq.skip 1
            |> Seq.isEmpty

          let signature = method.Name + "(" + (if hasOverloads then "..." else "()") + ")"
          let signatureText = textInline signature
          memberNameSummary(signatureText, findTag(input, method, ITag.TagType.Summary) |> Seq.tryExactlyOne)

        seq [ processReturn; processMethod ]
        |> Seq.map toElement
        |> Linq.ToReadOnlyCollection

      let grouped = methodsArray
                    |> Seq.groupBy(fun x -> x.Name)
                    |> Seq.sortBy fst
                    |> Seq.collect snd
                    |> Seq.map createRow 

      m_creator.CreateTable(grouped, [ "Returns"; "Name" ] |> createHeadings, "TODO", 3)

    let createPropertySection(isStatic, accessor, properties : seq<IProperty>) =
      let createRow(property : IProperty) =
        let processType =
          let content = textInline property.Type.DisplayName
          let link = m_linker.CreateLink(property.Type)
          if (System.String.IsNullOrEmpty link) then
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

      let grouped = properties
                    |> Seq.groupBy(fun x -> x.Name)
                    |> Seq.sortBy fst
                    |> Seq.collect snd
                    |> Seq.map createRow

      m_creator.CreateTable(grouped, [ "Type"; "Name"; "Methods" ] |> createHeadings, "TODO", 3)

    let processMembers (item) =
      let flatten item =
        let a = fst item
        snd item
        |> Seq.map(fun x -> (a, fst x, snd x))
      item
      |> Seq.map flatten
      |> Seq.collect(fun x -> x)

    let methods = input.Methods
                  |> groupMembers
                  |> processMembers
                  |> Seq.map (createMethodSection >> toElement)

    let properties = input.Properties
                     |> groupMembers
                     |> processMembers
                     |> Seq.map (createPropertySection >> toElement)

    seq [
      m_creator.CreateSection(methods, "Methods", 2) :> IElement;
      m_creator.CreateSection(properties, "Properties", 2) :> IElement;
    ]

  interface ITypePrinter with
    member __.Print(input : IInterface) = 
      if (isNull input) then
        raise (System.ArgumentNullException("input"))
      else
        let memberSection = printMemberTables input
        m_creator.CreatePage(null, memberSection, input.Name)
