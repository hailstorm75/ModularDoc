namespace MarkDoc.Linkers.Markdown

open MarkDoc.Members
open MarkDoc.Linkers
open MarkDoc.Members.ResolvedTypes
open MarkDoc.Members.Types
open MarkDoc.Members.Members
open System.Collections.Concurrent

/// <summary>
/// Markdown linker class
/// </summary>
type Linker(memberResolver, linkerSettings: ILinkerSettings) =
  let m_memberResolver : IResolver = memberResolver
  let m_anchors = ConcurrentDictionary<IMember, Lazy<string>>()
  let m_settings = linkerSettings :?> LinkerSettings
  let m_platform = match m_settings.Platform with
                   | "1" -> GitPlatform.GitLab
                   | "0" -> GitPlatform.GitHub
                   | _ -> GitPlatform.GitHub

  let structure =
    Structure.generateStructure(m_memberResolver.Types.Value, m_platform)

  let createLink (source: IType, target: IType) =
    Link.createLink(source, target, structure, m_platform)

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
    member _.CreateLinkToSourceCode(target: IMember) = createLinkToSourceCode target
