namespace ModularDoc.Linkers.Markdown

open System
open ModularDoc.Members
open ModularDoc.Linkers
open ModularDoc.Members.ResolvedTypes
open ModularDoc.Members.Types
open ModularDoc.Members.Members
open System.Collections.Concurrent

/// <summary>
/// Markdown linker class
/// </summary>
type Linker(memberResolver, linkerSettings: ILinkerSettings) =
  let evalBoolString (input: string): bool =
    let mutable result = false
    if bool.TryParse(input, &result) then
      result
    else
      false
  
  let m_memberResolver : IResolver = memberResolver
  let m_anchors = ConcurrentDictionary<IMember, Lazy<string>>()
  let m_settings = linkerSettings :?> LinkerSettings
  let m_platform = match m_settings.Platform with
                   | "3" -> GitPlatform.Bitbucket
                   | "1" -> GitPlatform.GitLab
                   | "0" -> GitPlatform.GitHub
                   | _ -> raise (NotSupportedException("Unsupported platform selected for the linking process"))
  let m_toWiki = evalBoolString m_settings.OutputTargetWiki
  let m_structured = evalBoolString m_settings.OutputStructured

  let structure =
    Structure.generateStructure m_memberResolver.Types.Value m_platform m_toWiki m_structured

  let createLink (source: IType, target: IType) =
    let link = Link.createLink(source, target, structure, m_platform)
    if m_toWiki then
      link
    else
      link + ".md"

  let createResLink (source: IType, target: IResType) =
    // If the target reference is not null..
    if target.Reference.Value |> (isNull >> not) then
      // create a link
      createLink(source, target.Reference.Value)
    // Otherwise..
    else
      // return an empty link
      ""

  let createAnchor page input =
    let mutable result = null
    // Ensure that everything is done lazily
    lazy(
      // If the member is known..
      if m_anchors.TryGetValue(input, &result) then
        // attempt to create an anchor for it
        match Anchor.createAnchor(result, createLink(page, page), m_platform) with
        | Some s -> s.Value
        | _ -> ""
      // Otherwise..
      else
        // return an empty anchor
        ""
    )

  let registerAnchor(target: IMember, anchor: Lazy<string>) =
    m_anchors.TryAdd(target, anchor) |> ignore
    
  let createLinkToSourceCode(target: IMember) =
    match SourceLinker.createSourceLink target m_platform m_settings.GitPlatformUser m_settings.GitPlatformRepository m_settings.GitPlatformBranch with
    | Some s -> s
    | None -> ""
    
  let getSourceCodeLinker: (IMember -> string) =
    if m_structured then
      createLinkToSourceCode
    else
      fun _ -> ""
      
  let sourceCodeLinkerMethod = getSourceCodeLinker
  
  member _.GetRawUrl() =
    match m_platform with
    | GitPlatform.GitLab -> $"https://gitlab.com/{m_settings.GitPlatformUser}/{m_settings.GitPlatformRepository}/-/raw/{m_settings.GitPlatformBranch}/"
    | GitPlatform.GitHub -> $"https://raw.githubusercontent.com/{m_settings.GitPlatformUser}/{m_settings.GitPlatformRepository}/{m_settings.GitPlatformBranch}/"
    | GitPlatform.Bitbucket -> $"https://bitbucket.org/{m_settings.GitPlatformUser}/{m_settings.GitPlatformRepository}/raw/{m_settings.GitPlatformBranch}/"
    | _ -> ""

  interface ILinker with
    /// <inheritdoc />
    member _.Paths with get() = structure

    /// <inheritdoc />
    member _.CreateLink(source: IType, target: IType) = createLink(source, target)
    /// <inheritdoc />
    member _.CreateLink(source: IType, target: IResType) = createResLink(source, target)
    /// <inheritdoc />
    member _.RegisterAnchor(target: IMember, anchor: Lazy<string>) = registerAnchor(target, anchor)
    /// <inheritdoc />
    member _.CreateAnchor(page: IType, target: IMember) = createAnchor page target
    /// <inheritdoc />
    member _.CreateLinkToSourceCode(target: IMember) = sourceCodeLinkerMethod target
