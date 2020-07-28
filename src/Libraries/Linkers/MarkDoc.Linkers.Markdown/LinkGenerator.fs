namespace MarkDoc.Linkers.Markdown

open MarkDoc.Members.Types
open System
open System.Collections.Generic

module private Link =
  let private processTarget source target =
    if source = target then
      target
    else
      let split (input: string) =
        input.Split("/", StringSplitOptions.RemoveEmptyEntries)
      let foldersSource = split source
      let foldersTarget = split target

      let index =
        seq [
          for i in 0 .. min foldersSource.Length foldersTarget.Length do
            yield foldersSource.[i].Equals(foldersTarget.[i], StringComparison.Ordinal)
        ]
        |> Seq.findIndex not

      let cdUp =
        seq [
          for _ in 0 .. index - 1 do
            yield "../"

          for i in index .. foldersTarget.Length - 1 do
            yield foldersTarget.[i]
        ]

      String.Join(String.Empty, cdUp)

  let createLink(source: IType, target: IType, structure: IReadOnlyDictionary<IType, string>, platform: GitPlatform) =
    let mutable resultTarget, resultSource = null, null
    if structure.TryGetValue(target, &resultTarget) && structure.TryGetValue(source, &resultSource) then
      processTarget resultSource resultTarget
    else
      ""
