namespace MarkDoc.Generator.Basic

open MarkDoc.Elements
open System

type TextType<'M when 'M :> ITextContent> =
  | Normal of text: string
  | Italic of text: string
  | Bold of text: string
  | Code of text: string
  | InlineCode of text: string
  | TextContent of content: 'M
  | LinkContent of content: TextType<'M> * reference: string Lazy
  | JoinedText of content: TextType<'M> seq * delimiter: string

module TextHelpers =
  let empty tools = tools.creator.CreateText("", IText.TextStyle.Normal)

  let normal text tools =
    tools.creator.CreateText(text, IText.TextStyle.Normal)

  let rec processTextNoStyle (textType: 'M TextType when 'M :> ITextContent) =
    match textType with
    | Normal text
    | Italic text
    | Bold text
    | Code text
    | InlineCode text -> text
    | JoinedText (content, delimiter) -> String.Join(delimiter, content |> Seq.map processTextNoStyle)

  let rec processText (textType: 'M TextType when 'M :> ITextContent) tools =
    let supplyTools input =
      input tools

    match textType with
    | Normal text -> tools.creator.CreateText(text, IText.TextStyle.Normal) :> ITextContent
    | Italic text -> tools.creator.CreateText(text, IText.TextStyle.Italic) :> ITextContent
    | Bold text -> tools.creator.CreateText(text, IText.TextStyle.Bold) :> ITextContent
    | Code text -> tools.creator.CreateText(text, IText.TextStyle.Code) :> ITextContent
    | InlineCode text -> tools.creator.CreateText(text, IText.TextStyle.CodeInline) :> ITextContent
    | TextContent content -> content :> ITextContent
    | LinkContent (content, reference) -> tools.creator.CreateLink(processText content tools :?> IText, reference) :> ITextContent
    | JoinedText (content, delimiter) -> tools.creator.JoinTextContent(content |> Seq.map (processText >> supplyTools), delimiter)
