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
type Linker(memberResolver) =
  let m_memberResolver : IResolver = memberResolver
  let m_anchors = new ConcurrentDictionary<IMember, Lazy<string>>()
  let m_platform = GitPlatform.GitLab

  let structure =
    Structure.generateStructure(m_memberResolver.Types.Value, m_platform)

  let createLink (source: IType, target: IType) =
    Link.createLink(source, target, structure, m_platform)

  let createResLink (source: IType, target: IResType) =
    // If the target reference is not null..
    if isNull target.Reference.Value |> not then
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

  interface ILinker with
    /// <inheritdoc />
    member __.Paths with get() = structure

    /// <inheritdoc />
    member __.CreateLink(source: IType, target: IType) = createLink(source, target)
    /// <inheritdoc />
    member __.CreateLink(source: IType, target: IResType) = createResLink(source, target)
    /// <inheritdoc />
    member __.RegisterAnchor(target: IMember, anchor: Lazy<string>) = registerAnchor(target, anchor) |> ignore
    /// <inheritdoc />
    member __.CreateAnchor(page: IType, target: IMember) = createAnchor page target
