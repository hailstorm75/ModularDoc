namespace MarkDoc.Linkers.Markdown

open MarkDoc.Linkers.Markdown
open MarkDoc.Members.Members
open System
open System.IO

module private SourceLinker =
  let mutable m_rootRepositoryDirectory : string = ""

  let nullToSome (v: 'M Nullable) =
    if v.HasValue then
      Some(v.Value)
    else
      None

  let ensureRootRepoDir (target: IMember) =
    let rec extractRootDir (source: string) =
      let dir = Path.GetDirectoryName source
      if isNull dir then
        None
      else
        let gitDir = Path.Combine(dir, ".git")
        if Directory.Exists gitDir then
          Some dir
        else
          extractRootDir dir

    match nullToSome target.LineSource with
    | Some lineSource -> 
      let t = lineSource.ToTuple()
      match t |> snd |> extractRootDir with
      | Some dir -> m_rootRepositoryDirectory <- dir
      | None -> ()

      Some t
    | None -> None

  let bitbucketLink (target: IMember) username repository branch  =
    None

  let githubLink (target: IMember) username repository branch =
    let format = "https://github.com/{0}/{1}/blob/{2}/{3}#L{4}"

    match ensureRootRepoDir target with
    | Some x -> Some(String.Format(format, [ username; repository; branch; (snd x).Remove(0, m_rootRepositoryDirectory.Length); x |> fst |> string ]))
    | None -> None

  let gitlabLink (target: IMember) username repository branch =
     let format = "https://gitlab.com/{0}/{1}/-/blob/{2}/{3}#L{4}"

     match ensureRootRepoDir target with
     | Some x -> Some(String.Format(format, [ username; repository; branch; (snd x).Remove(0, m_rootRepositoryDirectory.Length); x |> fst |> string ]))
     | None -> None

  let azureLink (target: IMember) username repository branch =
    None

  let createSourceLink (target: IMember) (platform: GitPlatform) (username: string) (repository: string) (branch: string) =
    match platform with
    | GitPlatform.BitBucket -> bitbucketLink
    | GitPlatform.GitHub -> githubLink
    | GitPlatform.GitLab -> gitlabLink
    | GitPlatform.Azure -> azureLink
    | _ -> (fun _ _ _ _ -> None)
    <| target <| username <| repository <| branch
