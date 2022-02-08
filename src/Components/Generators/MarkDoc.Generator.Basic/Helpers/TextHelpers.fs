namespace MarkDoc.Generator.Basic

open MarkDoc.Elements
open System

/// <summary>
/// Text styles and wrapper types
/// </summary>
/// <typeparam name="M">Generic type for text wrappers</typeparam>
type TextType<'M when 'M :> ITextContent> =
  /// <summary>
  /// Normal unstylized text
  /// </summary>
  | Normal of text: string
  /// <summary>
  /// Diagram content
  /// </summary>
  | Diagram of text: string
  /// <summary>
  /// Italic text
  /// </summary>
  | Italic of text: string
  /// <summary>
  /// Bold text
  /// </summary>
  | Bold of text: string
  /// <summary>
  /// Multiline code text
  /// </summary>
  | Code of text: string
  /// <summary>
  /// Inline code text
  /// </summary>
  | InlineCode of text: string
  /// <summary>
  /// Wrapper for <see cref="ITextContent"/>
  /// </summary>
  | TextContent of content: 'M
  /// <summary>
  /// Link text
  /// </summary>
  | LinkContent of content: TextType<'M> * reference: string Lazy
  /// <summary>
  /// Joined text
  /// </summary>
  | JoinedText of content: TextType<'M> seq * delimiter: string

module internal TextHelpers =
  /// <summary>
  /// Gets an empty text element
  /// </summary>
  /// <param name="tools">Tools for creating the text element</param>
  /// <returns>Empty text element</returns>
  let empty tools = tools.creator.CreateText("", IText.TextStyle.Normal)

  /// <summary>
  /// Creates an unstylized text element from the given <paramref name="text"/>
  /// </summary>
  /// <param name="text">Text source</param>
  /// <param name="tools">Tools for creating the text element</param>
  /// <returns>Normal text element</returns>
  let normal text tools = tools.creator.CreateText(text, IText.TextStyle.Normal)

  /// <summary>
  /// Create text elements for the given <paramref name="headings"/>
  /// </summary>
  /// <param name="headings">Headings to process</param>
  /// <param name="tools">Tools for creating the text element</param>
  /// <returns>Text elements of <paramref name="headings"/></returns>
  let createHeadings headings tools = headings |> Seq.map (fun text -> normal text tools)

  /// <summary>
  /// Process the given text <paramref name="input"/> into a raw string
  /// </summary>
  /// <param name="input">Text type input</param>
  /// <typeparam name="M">Generic type for text wrappers</typeparam>
  /// <returns>Raw string</returns>
  let rec processTextNoStyle (input: 'M TextType when 'M :> ITextContent) =
    // Process the given input based on its type
    match input with
    | Normal text
    | Diagram text
    | Italic text
    | Bold text
    | Code text
    | InlineCode text -> text
    | LinkContent (content, _) -> content |> processTextNoStyle 
    | JoinedText (content, delimiter) -> String.Join(delimiter, content |> Seq.map processTextNoStyle |> Seq.filter (String.IsNullOrEmpty >> not))
    | _ -> ""

  /// <summary>
  /// Process the given text <paramref name="textType"/> into a text content element
  /// </summary>
  /// <param name="textType">Text type input</param>
  /// <param name="tools">Tools for creating the text element</param>
  /// <typeparam name="M">Generic type for text wrappers</typeparam>
  /// <returns>Text content element</returns>
  let rec processText (textType: 'M TextType when 'M :> ITextContent) tools =
    let applyTools input = input tools

    // Process the given input based on its type
    match textType with
    | Normal text -> tools.creator.CreateText(text, IText.TextStyle.Normal) :> ITextContent
    | Diagram text -> tools.creator.CreateDiagram(text)
    | Italic text -> tools.creator.CreateText(text, IText.TextStyle.Italic) :> ITextContent
    | Bold text -> tools.creator.CreateText(text, IText.TextStyle.Bold) :> ITextContent
    | Code text -> tools.creator.CreateText(text, IText.TextStyle.Code) :> ITextContent
    | InlineCode text -> tools.creator.CreateText(text, IText.TextStyle.CodeInline) :> ITextContent
    | TextContent content -> content :> ITextContent
    | LinkContent (content, reference) -> tools.creator.CreateLink(processText content tools :?> IText, reference) :> ITextContent
    | JoinedText (content, delimiter) -> tools.creator.JoinTextContent(content |> Seq.map (processText >> applyTools), delimiter)
