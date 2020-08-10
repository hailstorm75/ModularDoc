namespace MarkDoc.Linkers.Markdown

open System.Text.RegularExpressions
open MarkDoc.Members.Types

module private Anchor =
  let normalizerRegex = new Regex(@"(?<Gwh>\s)|(?<Gsym>[^a-z0-9]*)")
  let normalizerDictionary (x: Match) =
    match x.Groups |> Seq.tryFind (fun x -> x.Success && x.Name.[0] = 'G') with
    | Some s ->
      match s.Name with 
      | "Gwh" -> "-"
      | "Gsym" -> ""
      | _ -> x.Value
    | None -> x.Value

  let normalizeAnchor (anchor: string) =
    normalizerRegex.Replace(anchor.ToLowerInvariant(), normalizerDictionary)

  let private bitbucketAnchor (input: string Lazy, page: string) =
    // TODO: Not supported
    lazy("") |> Some

  let private githubAnchor (input: string Lazy, page: string) =
    lazy(page + "#" + normalizeAnchor input.Value) |> Some

  let private gitlabAnchor (input: string Lazy, page: string) =
    lazy("#" + normalizeAnchor input.Value) |> Some

  let private azureAnchor (input: string Lazy, page: string) =
    lazy("?anchor=") |> Some

  let createAnchor(input: string Lazy, pageName: string, platform: GitPlatform) =
    let creator = match platform with
                  | GitPlatform.BitBucket -> bitbucketAnchor
                  | GitPlatform.GitHub -> githubAnchor
                  | GitPlatform.GitLab -> gitlabAnchor
                  | GitPlatform.Azure -> azureAnchor
                  | _ -> (fun _ -> None)
    (input, pageName) |> creator

