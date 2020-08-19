namespace MarkDoc.Generator.Basic

open MarkDoc.Documentation.Tags
open System
open MarkDoc.Helpers
open MarkDoc.Elements

module ContentProcessor =
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
        -> Some(TypeHelpers.processReference(input, inner.Reference, tools) |> TextElement)
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


