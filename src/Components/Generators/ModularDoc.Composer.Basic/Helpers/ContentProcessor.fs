﻿namespace ModularDoc.Composer.Basic

open ModularDoc.Documentation.Tags
open ModularDoc.Elements
open ModularDoc.Helpers
open System

module internal ContentProcessor =
  /// <summary>
  /// Composes provided <paramref name="content"/> to elements
  /// </summary>
  /// <param name="input">Content source type</param>
  /// <param name="content">Content to process</param>
  /// <param name="tools">Tools for processing content</param>
  /// <returns>Composed elements</returns>
  let rec processContent (input, content, tools) =
    let applyTools input = input tools
    let toSomeText input = input |> TextElement |> Some

    let getInlineText (tag: IInnerTag) =
      tag.Content
      // Get content that is text
      |> Seq.where(fun x -> x :? ITextTag)
      // Get the text
      |> Seq.map (fun x -> (x :?> ITextTag).Content)

    let processColumn(column: seq<IContent>) =
      column
      // Extract inner content
      |> Seq.map (fun content -> processContent(input, content, tools))
      // Filter out invalid content
      |> SomeHelpers.whereSome
      // Initialize content into elements
      |> Seq.map (ElementHelpers.initialize >> applyTools)
      
    let processSeeOrSeeAlso(tag: IInnerTag) =
      let items = (tag.Reference, tag.Link, tag.Content)
      match items with
      | reference, link, _ when link.Length = 0 && reference.Length = 0 ->
        None
      | reference, link, _ when link.Length = 0 ->
        TypeHelpers.processReference input reference tools |> toSomeText
      | reference, link, content when reference.Length <> 0 && link.Length <> 0 && content.Count = 0 ->
        let r = TypeHelpers.processReference input reference tools
        let l = JoinedText (seq [Normal "("; LinkContent (Normal link, lazy link); Normal ")"], "")
        
        JoinedText(seq [r; l], " ") |> toSomeText
      | reference, link, content when reference.Length <> 0 && link.Length <> 0 && content.Count <> 0 ->
        let r = TypeHelpers.processReference input reference tools
        let l = JoinedText (seq [Normal "("; LinkContent (getInlineText tag |> Seq.exactlyOne |> Normal, lazy link); Normal ")"], "")
        
        JoinedText(seq [r; l], " ") |> toSomeText
      | _, link, content when link.Length <> 0 && content.Count = 0 ->
        LinkContent (Normal link, lazy link) |> toSomeText
      | _, link, content when link.Length <> 0 && content.Count <> 0 ->
        LinkContent (getInlineText tag |> Seq.exactlyOne |> Normal, lazy link) |> toSomeText
      | _ -> None

    // Process the content based on its type
    match content with
    | :? ITextTag as text
      -> text.Content |> Normal |> toSomeText
    | :? IInnerTag as inner ->
      match inner.Type with
      | IInnerTag.InnerTagType.CodeSingle
        -> getInlineText inner |> Seq.exactlyOne |> InlineCode |> toSomeText
      | IInnerTag.InnerTagType.Code
        -> getInlineText inner |> Seq.exactlyOne |> Code |> toSomeText
      | IInnerTag.InnerTagType.ParamRef
      | IInnerTag.InnerTagType.TypeRef
        -> inner.Reference |> InlineCode |> toSomeText
      | IInnerTag.InnerTagType.See
      | IInnerTag.InnerTagType.SeeAlso
        -> processSeeOrSeeAlso inner
      | IInnerTag.InnerTagType.Para
        -> Environment.NewLine + Environment.NewLine |> Normal |> toSomeText
      | _ -> None
    | :? IListTag as list ->
      match list.Type with
      | IListTag.ListType.Table ->
        // Get table content
        let content = list.Rows |> Seq.map (processColumn >> Linq.ToReadOnlyCollection)
        // Get table headings
        let headings =
          list.Headings
          // Extract inner content
          |> Seq.map (fun content -> processContent(input, content, tools))
          // Filter out invalid content
          |> SomeHelpers.whereSome
          // Transform the content to text elements
          |> Seq.filter ElementHelpers.isTextElement
          // Initialize the elements
          |> Seq.map(fun element -> element |> (ElementHelpers.initialize >> applyTools) :?> IText)
        // Compose the table
        Table(content, headings, "", 0) |> Some
      | _ ->
        let listType (listType: IListTag.ListType) =
          match listType with
          | IListTag.ListType.Bullet -> IList.ListType.Dotted
          | IListTag.ListType.Number -> IList.ListType.Numbered
          | _ -> raise (NotSupportedException())

        // Get list content
        let content =
          list.Rows
          // Flatten the sequence
          |> Seq.collect id
          // Extract inner content
          |> Seq.map (fun content -> processContent(input, content, tools))
          // Filter out invalid content
          |> SomeHelpers.whereSome
          // Initialize content into elements
          |> Seq.map (ElementHelpers.initialize >> applyTools)
        // Compose the list
        ListElement(content, listType list.Type, "", 0) |> Some
    | _ -> None


