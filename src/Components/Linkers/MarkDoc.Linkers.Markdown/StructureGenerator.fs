namespace MarkDoc.Linkers.Markdown

open System.Collections.Generic
open MarkDoc.Members.Types
open System

module private Structure =
  /// <summary>
  /// Helper method for generating link structures
  /// </summary>
  /// <param name="input">Dictionary of types group by their namespaces</param>
  /// <param name="platform">Platform for which the structure is to be generated for</param>
  /// <returns>Paired types to their output location</returns>
  let generateStructure (input: IReadOnlyDictionary<string, IReadOnlyCollection<IType>>, platform: GitPlatform) =
    let result = new Dictionary<IType, string>()

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
        | _ -> raise (new NotSupportedException())

      (input, processor input.TypeNamespace + TypeHelper.getName input)

    input
    // Process types
    |> Seq.map (fun x -> x.Value |> Seq.map createStructure)
    // Flatten the collection
    |> Seq.collect id
    // Populate the resulting structure
    |> Seq.iter addToResult

    result :> IReadOnlyDictionary<IType, string>

