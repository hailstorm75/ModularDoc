namespace MarkDoc.Linkers.Markdown

open System.Text.RegularExpressions

module private Helpers =
  let normalizerRegex = new Regex(@"(?<wh>\s)|(?<sym>[^A-Za-z0-9]*)")
  let normalizerDictionary (x: Match) =
    match x.Groups |> Seq.tryFind (fun x -> x.Success) with
    | Some as s ->
      match s.Value.Name with 
      | "wh" -> "-"
      | "sym" -> ""
      | _ -> x.Value
    | None -> x.Value

  let normalizeAnchor (anchor: string) =
    normalizerRegex.Replace(anchor.ToLowerInvariant(), normalizerDictionary)

  let private bitbucketAnchor (input: string Lazy) =
    // TODO: Not supported
    lazy("") |> Some

  let private githubAnchor (input: string Lazy) =
    lazy("wiki#" + normalizeAnchor input.Value) |> Some

  let private gitlabAnchor (input: string Lazy) =
    lazy("#" + normalizeAnchor input.Value) |> Some

  let private azureAnchor (input: string Lazy) =
    lazy("?anchor=") |> Some

  let createAnchor(input: string Lazy, platform: GitPlatform) =
    let creator = match platform with
                  | GitPlatform.BitBucket -> bitbucketAnchor
                  | GitPlatform.GitHub -> githubAnchor
                  | GitPlatform.GitLab -> gitlabAnchor
                  | GitPlatform.Azure -> azureAnchor
                  | _ -> (fun _ -> None)
    input |> creator

