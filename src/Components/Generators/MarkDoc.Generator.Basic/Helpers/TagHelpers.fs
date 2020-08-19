namespace MarkDoc.Generator.Basic

open MarkDoc.Documentation.Tags
open System.Collections.Generic
open MarkDoc.Elements
open MarkDoc.Members.Types
open MarkDoc.Documentation
open MarkDoc.Members.Members

module internal TagHelpers =
  let tagShort input (tag: ITag) tools =
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
                    |> Seq.map (fun x -> ContentProcessor.processContent(input, x, tools))
    let content = seq [readMore]
                  |> Seq.append processed
                  |> SomeHelpers.whereSome
                  |> ElementHelpers.getTextContent

    JoinedText (content, " ")

  let tagFull (input: IType) (tag: ITag) tools =
    let content = tag.Content
                  |> Seq.map (fun x -> ContentProcessor.processContent(input, x, tools))
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

  let findTypeTag (input: IType) (tag: ITag.TagType) tools =
    seq [
      let mutable typeDoc: IDocElement = null
      if (tools.docResolver.TryFindType(input, &typeDoc)) then
        let mutable result: IReadOnlyCollection<ITag> = null
        if (typeDoc.Documentation.Tags.TryGetValue(tag, &result)) then
          result
    ]
    |> Seq.collect id

  let findTag (input: IType) (mem: IMember) (tag: ITag.TagType) tools =
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


