namespace MarkDoc.Generator.Basic

open MarkDoc.Elements
open System.Collections.Generic

type internal Element<'M when 'M :> ITextContent> =
  | TextElement of content: TextType<'M>
  | LinkElement of content: TextType<'M> * reference: string
  | ListElement of content: IElement seq * listType: IList.ListType * heading: string * level: int
  | Section of content: IElement seq * heading: string * level: int
  | Table of content: IElement IReadOnlyCollection seq * headings: IText seq * heading: string * level: int
  | Page of content: IElement seq * heading: string * level: int

module internal ElementHelpers =
  let isTextElement input =
    match input with
    | TextElement _ -> true
    | _ -> false

  let getTextContent (input: 'M Element seq when 'M :> ITextContent) =
    let extractor input =
      match input with
      | TextElement content -> Some content
      | _ -> None

    input
    // Extract elements that are text elements
    |> Seq.map extractor
    // Filter out the non-text elements
    |> SomeHelpers.whereSome

  let toElement (input: 'M when 'M :> IElement) = input :> IElement

  let initialize element tools =
    match element with
    | TextElement content -> TextHelpers.processText content tools :> IElement
    | LinkElement (content, reference) -> tools.creator.CreateLink(TextHelpers.processText content tools :?> IText, lazy(reference)) :> IElement
    | ListElement (content, listType, heading, level) -> tools.creator.CreateList(content, listType, heading, level) :> IElement
    | Section (content, heading, level) -> tools.creator.CreateSection(content, heading, level) :> IElement
    | Table (content, headings, heading, level) -> tools.creator.CreateTable(content, headings, heading, level) :> IElement
    | Page (content, heading, level) -> tools.creator.CreatePage(null, content, heading, level) :> IElement


