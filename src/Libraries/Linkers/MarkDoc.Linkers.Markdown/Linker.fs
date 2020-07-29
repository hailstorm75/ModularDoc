namespace MarkDoc.Linkers.Markdown

open MarkDoc.Members
open MarkDoc.Linkers
open MarkDoc.Members.ResolvedTypes
open MarkDoc.Members.Types
open MarkDoc.Members.Members
open System.Collections.Concurrent

type Linker(memberResolver) =
  let m_memberResolver : IResolver = memberResolver
  let m_anchors = new ConcurrentDictionary<IMember, Lazy<string>>()
  let m_platform = GitPlatform.GitLab

  let structure =
    Structure.generateStructure(m_memberResolver.Types.Value, m_platform)

  let createLink (source: IType, target: IType) =
    Link.createLink(source, target, structure, m_platform)

  let createResLink (source: IType, target: IResType) =
    if isNull target.Reference.Value |> not then
      createLink(source, target.Reference.Value)
    else
      ""

  let createAnchor page input =
    let mutable result = null
    lazy(
      if m_anchors.TryGetValue(input, &result) then
        match Anchor.createAnchor(result, createLink(page, page), m_platform) with
        | Some as s -> s.Value.Value
        | _ -> ""
      else
        ""
    )

  let registerAnchor(target: IMember, anchor: Lazy<string>) =
    m_anchors.TryAdd(target, anchor) |> ignore

  interface ILinker with
    member __.Paths with get() = structure

    member __.CreateLink(source: IType, target: IType) = createLink(source, target)
    member __.CreateLink(source: IType, target: IResType) = createResLink(source, target)
    member __.RegisterAnchor(target: IMember, anchor: Lazy<string>) = registerAnchor(target, anchor) |> ignore
    member __.CreateAnchor(page: IType, target: IMember) = createAnchor page target
