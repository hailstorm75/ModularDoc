namespace MarkDoc.Generator.Basic

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
      |> Seq.map m_creator.CreateText

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
          let content = m_creator.CreateText(name, IText.TextStyle.CodeInline)
          if isNull method.Returns then
            content :> ITextContent
          else
            let link = m_linker.CreateLink method.Returns
            if System.String.IsNullOrEmpty link then
              content :> ITextContent
            else
              m_creator.CreateLink(content, link) :> ITextContent

        let processMethod : ITextContent = 
          let hasOverloads =
            methodsArray
            |> Seq.where(fun x -> x.Name = method.Name)
            |> Seq.skip 1
            |> Seq.isEmpty
          let signature = method.Name + "(" + (if hasOverloads then "..." else "()") + ")"
          let summary = findTag(input, method, ITag.TagType.Summary) |> Seq.tryExactlyOne

          let signatureText = m_creator.CreateText(signature, IText.TextStyle.CodeInline)
          m_creator.JoinTextContent([signatureText], "<br>")

        [ processReturn; processMethod ]
        |> Seq.map(fun x -> x :> IElement)
        |> Linq.ToReadOnlyCollection

      let grouped = methodsArray
                    |> Seq.groupBy(fun x -> x.Name)
                    |> Seq.sortBy fst
                    |> Seq.collect snd
                    |> Seq.map createRow 

      m_creator.CreateTable(grouped, [ "Returns"; "Name" ] |> createHeadings, "TODO", 3)

    let processMembers (item) =
      let flatten item =
        let a = fst item
        snd item
        |> Seq.map(fun x -> (a, fst x, snd x))
      item
      |> Seq.map flatten
      |> Seq.collect(fun x -> x)

    let a = input.Methods
            |> groupMembers
            |> processMembers
            |> Seq.map createMethodSection
            |> Seq.map(fun x-> x :> IElement)

    [ m_creator.CreateSection(a, "Methods", 2) :> IElement ]

  interface ITypePrinter with
    member __.Print(input : IInterface) = 
      if (isNull input) then
        raise (System.ArgumentNullException("input"))
      else
        let memberSection = printMemberTables input
        m_creator.CreatePage(null, memberSection, input.Name)
