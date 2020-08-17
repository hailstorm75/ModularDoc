namespace MarkDoc.Generator.Basic

open MarkDoc.Elements

module internal ElementHelpers =
  type Element =
    | Section of content: IElement seq * heading: string * level: int
    | Page of content: IElement seq * heading: string * level: int

  let initialize element tools =
    match element with
    | Section (content, heading, level) -> tools.creator.CreateSection(content, heading, level) :> IElement
    | Page (content, heading, level) -> tools.creator.CreatePage(null, content, heading, level) :> IElement


