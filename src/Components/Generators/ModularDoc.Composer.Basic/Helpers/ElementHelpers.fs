namespace ModularDoc.Composer.Basic

open ModularDoc.Elements
open System.Collections.Generic

/// <summary>
/// Element types
/// </summary>
/// <typeparam name="M">Wraped text type</typeparam>
type internal Element<'M when 'M :> ITextContent> =
  /// <summary>
  /// Element containing text
  /// </summary>
  | TextElement of content: TextType<'M>
  /// <summary>
  /// Element containing a diagram
  /// </summary>
  | DiagramElement of name: string * content: string
  /// <summary>
  /// Element containing a link
  /// </summary>
  | LinkElement of content: TextType<'M> * reference: string
  /// <summary>
  /// Element containing a list
  /// </summary>
  | ListElement of content: IElement seq * listType: IList.ListType * heading: string * level: int
  /// <summary>
  /// Element containing a section
  /// </summary>
  | Section of content: IElement seq * heading: string * level: int
  /// <summary>
  /// Element containing a table
  /// </summary>
  | Table of content: IElement IReadOnlyCollection seq * headings: IText seq * heading: string * level: int
  /// <summary>
  /// Element containing a page
  /// </summary>
  | Page of content: IElement seq * heading: string * level: int

module internal ElementHelpers =
  /// <summary>
  /// Checks whether the given <paramref name="input"/> is a <see cref="TextElement"/>
  /// </summary>
  /// <param name="input">Item to check</param>
  /// <returns>True is the given <paramref name="input"/> is a <see cref="TextElement"/></returns>
  let isTextElement input =
    match input with
    | TextElement _ -> true
    | _ -> false

  /// <summary>
  /// Gets the content of the provided sequence of text elements
  /// </summary>
  /// <param name="text">Text for processing</param>
  /// <typeparam name="M">Wraped text type</typeparam>
  /// <returns>Extracted text content</returns>
  let getTextContent (text: 'M Element seq when 'M :> ITextContent) =
    let extractor element =
      // Extracts text if the element is a text element
      match element with
      | TextElement content -> Some content
      | _ -> None

    text
    // Extract elements that are text elements
    |> Seq.map extractor
    // Filter out the non-text elements
    |> SomeHelpers.whereSome

  /// <summary>
  /// Casts given <paramref name="input"/> to an <see cref="IElement"/> type
  /// </summary>
  /// <param name="input">Item for processing</param>
  /// <typeparam name="M">Wrapped text type</typeparam>
  /// <returns>Input element after casting</returns>
  let toElement (input: 'M when 'M :> IElement) = input :> IElement

  /// <summary>
  /// Initializes given sequence of <paramref name="element"/>
  /// </summary>
  /// <param name="element">Element instance to initialize</param>
  /// <param name="tools">Tools for creating the text element</param>
  /// <returns>Initialized element</returns>
  let initialize element tools =
    // Initializes given element based on its type
    match element with
    | TextElement content -> TextHelpers.processText content tools :> IElement
    | DiagramElement (name, content) -> tools.creator.CreateDiagram(name, content) :> IElement
    | LinkElement (content, reference) -> tools.creator.CreateLink(TextHelpers.processText content tools :?> IText, lazy reference) :> IElement
    | ListElement (content, listType, heading, level) -> tools.creator.CreateList(content, listType, heading, level) :> IElement
    | Section (content, heading, level) -> tools.creator.CreateSection(content, heading, level) :> IElement
    | Table (content, headings, heading, level) -> tools.creator.CreateTable(content, headings, heading, level) :> IElement
    | Page (content, heading, level) -> tools.creator.CreatePage(null, content, heading, level) :> IElement

