namespace MarkDoc.Linkers.Markdown

open MarkDoc.Members.Types
open System.Collections.Generic

module private Link =
  let createLink(source: IType, target: IType, structure: IReadOnlyDictionary<IType, string>, platform: GitPlatform) =
    let mutable result = null
    if structure.TryGetValue(target, &result) then
      result
    else
      ""
