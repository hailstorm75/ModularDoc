namespace MarkDoc.Generator.Basic

open System.Collections.Generic
open MarkDoc.Members.Members
open MarkDoc.Documentation.Tags
open MarkDoc.Documentation
open MarkDoc.Elements

module internal TagHelpers =
  /// <summary>
  /// Gets the shortened version of the given <paramref name="tag"/> for the specified <paramref name="input"/>
  /// </summary>
  /// <param name="input"></param>
  /// <param name="tag">Documentation tag to process</param>
  /// <param name="tools">Tools for retrieving the documentation</param>
  /// <returns>Processed tag into elements</returns>
  let tagShort input (tag: ITag) tools =
    // Number of valid tags
    let count =
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

      // Try to find the first invalid tag
      match (tag.Content |> Seq.tryFindIndex isInvalid) with
      // If there are no invalid tags return the count of all tags
      | None -> tag.Content.Count
      // If there are invalid tags return the count of valid tags
      | Some count -> count

    // Get the read more indicator
    let readMore =
      // If the content was cut off..
      if (count <> tag.Content.Count) then
        // indicate there is more to read
        Some("..." |> Normal |> TextElement)
      // Otherwise..
      else
        // there is no read more
        None

    // Get tag content
    let processed = tag.Content
                    // Get tag content untill the first invalid tag
                    |> Seq.take count
                    // Convert tag content to elements
                    |> Seq.map (fun content -> ContentProcessor.processContent(input, content, tools))

    // Compose the content
    let content = seq [ readMore ]
                  // Append processed content followed by the readme
                  |> Seq.append processed
                  // Filter out invalid content
                  |> SomeHelpers.whereSome
                  // Compose the text elements
                  |> ElementHelpers.getTextContent

    // Join the content
    JoinedText (content, " ")

  /// <summary>
  /// Gets the full version of the given <paramref name="tag"/> for the specified <paramref name="input"/>
  /// </summary>
  /// <param name="input"></param>
  /// <param name="tag">Documentation tag to process</param>
  /// <param name="tools">Tools for retrieving the documentation</param>
  /// <returns>Processed tag into elements</returns>
  let tagFull input (tag: ITag) tools =
    // Get the tag content
    let elements = tag.Content
                  // Convert tag content to elements
                  |> Seq.map (fun content -> ContentProcessor.processContent(input, content, tools))
                  // Filter out invalid elements
                  |> SomeHelpers.whereSome
                  // Compose the elements
                  |> Seq.map (fun element -> ElementHelpers.initialize element tools)

    // Buffer for text elements
    let list = new LinkedList<ITextContent>()
    // Process the tag content
    let content = seq [
      // For each element..
      for element in elements do
        // if the element is text..
        if (element :? ITextContent && (not (element :? IText) || (element :?> IText).Style <> IText.TextStyle.Code)) then
          // add it to the buffer
          list.AddLast (element :?> ITextContent) |> ignore
        // otherwise if the element is not text and the buffer is empty..
        elif (list.Count = 0) then
          // return the element as is
          yield element
        // otherwise..
        else
          // join the buffered text
          let joined = tools.creator.JoinTextContent(list, " ") |> ElementHelpers.toElement
          // clear the buffer
          list.Clear()

          // return the joined text
          yield joined
          // return the element
          yield element
    ]

    // If there is no tag content..
    if (Seq.isEmpty content) then
      // compose and return the buffered text content
      seq [ tools.creator.JoinTextContent(list, " ") |> ElementHelpers.toElement ]
    // Otherwise..
    else
      // return the processed content
      content

  /// <summary>
  /// Find documentation of the given <paramref name="input"/> type of specified <paramref name="tag"/> type
  /// </summary>
  /// <param name="input">Type for which to retrieve the documentation</param>
  /// <param name="tag">Documentation tag type to retrieve</param>
  /// <param name="tools">Tools for retrieving the documentation</param>
  /// <returns>Found tags</returns>
  let findTypeTag input tag tools =
    seq [
      // Prepare output variable
      let mutable typeDoc: IDocElement = null
      // If documentation for the given type exists..
      if (tools.docResolver.TryFindType(input, &typeDoc)) then
        // prepare output variable
        let mutable result: IReadOnlyCollection<ITag> = null
        // if documentation of specified type exists..
        if (typeDoc.Documentation.Tags.TryGetValue(tag, &result)) then
          // return the documentation tag
          result
    ]
    // Flatten the collection
    |> Seq.collect id

  /// <summary>
  /// Find documentation of the given member of specified <paramref name="tag"/> type
  /// </summary>
  /// <param name="input">Type inside which the member is declared</param>
  /// <param name="mem">Member for which to retrieve the documentation</param>
  /// <param name="tag">Documentation tag type to retrieve</param>
  /// <param name="tools">Tools for retrieving the documentation</param>
  /// <returns>Found tags</returns>
  let findTag input (mem: IMember) tag tools =
    seq [
      // Prepare output variable
      let mutable typeDoc: IDocElement = null
      // If documentation for the given type exists..
      if (tools.docResolver.TryFindType(input, &typeDoc)) then
        // prepare output variable
        let mutable memberDoc: IDocMember = null
        // if the type has the given member defined..
        if (typeDoc.Members.Value.TryGetValue(mem.RawName, &memberDoc)) then
          // prepare output variable
          let mutable result: IReadOnlyCollection<ITag> = null
          // if documentation of specified type exists..
          if (memberDoc.Documentation.Tags.TryGetValue(tag, &result)) then
            // return the documentation tag
            result
    ]
    // Flatten the collection
    |> Seq.collect id
