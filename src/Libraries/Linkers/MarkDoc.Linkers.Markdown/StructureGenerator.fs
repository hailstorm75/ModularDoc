namespace MarkDoc.Linkers.Markdown

open System.Collections.Generic
open MarkDoc.Members.Types
open System

module private Structure =
  let generateStructure (input: IReadOnlyDictionary<string, IReadOnlyCollection<IType>>, platform: GitPlatform) =
    let result = new Dictionary<IType, string>()
    let getName (input: IType) = 
      match input with
      | :? IInterface as i ->
        i.Name + new System.String('T', i.Generics.Count)
      | _ ->
        input.Name

    let addToResult x =
      result.Add(fst x, snd x)

    let createStructure (input: IType) =
      let processor = 
        match platform with
        | GitPlatform.GitLab
        | GitPlatform.BitBucket
        | GitPlatform.Azure ->
          fun (space: string) -> space.ToLowerInvariant().Replace('.', '/') + "/"
        | GitPlatform.GitHub ->
          fun (space: string) -> space.ToLowerInvariant().Replace(".", "") + "-"
        | _ -> raise (new Exception())

      (input, processor input.TypeNamespace + getName input)

    input
    |> Seq.map (fun x -> x.Value |> Seq.map createStructure)
    |> Seq.collect id
    |> Seq.iter addToResult

    result :> IReadOnlyDictionary<IType, string>

