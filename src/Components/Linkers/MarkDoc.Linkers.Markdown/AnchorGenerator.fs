namespace MarkDoc.Linkers.Markdown

open System.Text.RegularExpressions

module private Anchor =
  let private normalizerRegex = Regex(@"(?<Gwh>\s)|(?<Gsym>[^a-z0-9]*)")

  let private normalizerDictionary (x: Match) =
    match x.Groups |> Seq.tryFind (fun x -> x.Success && x.Name.[0] = 'G') with
    | Some s ->
      match s.Name with 
      | "Gwh" -> "-"
      | "Gsym" -> ""
      | _ -> x.Value
    | None -> x.Value

  let private normalizeAnchor (anchor: string) =
    normalizerRegex.Replace(anchor.ToLowerInvariant(), normalizerDictionary)

  let private bitbucketAnchor (_: string Lazy, _: string) =
    // TODO: Not supported
    None

  let private githubAnchor (input: string Lazy, page: string) =
    lazy(page + "#" + normalizeAnchor input.Value) |> Some

  let private gitlabAnchor (input: string Lazy, _: string) =
    lazy("#" + normalizeAnchor input.Value) |> Some

  let private azureAnchor (_: string Lazy, _: string) =
    lazy("?anchor=") |> Some

  /// <summary>
  /// Creates an anchor for a given <paramref name="input"/> and <paramref name="platform"/>
  /// </summary>
  /// <param name="input">Input member to process</param>
  /// <param name="pageName">Page link</param>
  /// <param name="platform">Platform for which the structure is to be generated for</param>
  let createAnchor(input: string Lazy, pageName: string, platform: GitPlatform) =
    let creator = match platform with
                  | GitPlatform.BitBucket -> bitbucketAnchor
                  | GitPlatform.GitHub -> githubAnchor
                  | GitPlatform.GitLab -> gitlabAnchor
                  | GitPlatform.Azure -> azureAnchor
                  | _ -> (fun _ -> None)
    (input, pageName) |> creator

