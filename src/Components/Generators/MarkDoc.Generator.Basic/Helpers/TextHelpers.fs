namespace MarkDoc.Generator.Basic

open MarkDoc.Elements

type TextType =
  | Normal of text: string
  | Italic of text: string
  | Bold of text: string
  | Code of text: string
  | InlineCode of text: string
  | Joined of content: ITextContent

module TextHelpers =
  let processText textType tools =
    match textType with
    | Normal text -> tools.creator.CreateText(text, IText.TextStyle.Normal) :> ITextContent
    | Italic text -> tools.creator.CreateText(text, IText.TextStyle.Italic) :> ITextContent
    | Bold text -> tools.creator.CreateText(text, IText.TextStyle.Bold) :> ITextContent
    | Code text -> tools.creator.CreateText(text, IText.TextStyle.Code) :> ITextContent
    | InlineCode text -> tools.creator.CreateText(text, IText.TextStyle.CodeInline) :> ITextContent
    | Joined content -> content
