namespace MarkDoc.Generator.Basic

open MarkDoc.Elements
open MarkDoc.Members.Types

module internal ComposerHelpers =
  let printIntroduction input =
    Seq.empty |> Some
  let printMemberTables input =
    Seq.empty |> Some
  let printDetailed input =
    Seq.empty |> Some

  let composeSections input level =
    let createSection(x : seq<IElement>, y : string) =
      ElementHelpers.Element.Section(x, y, level)

    input
    |> Seq.filter SomeHelpers.whereSome2
    |> Seq.map (SomeHelpers.get2 >> createSection >> ElementHelpers.initialize)

  let composeContent (input: IType) =
    let result = seq [
                    (printIntroduction input, "Description");
                    (printMemberTables input, "Members");
                    (printDetailed input, "Details")
                  ]
                  |> composeSections

    result 1

