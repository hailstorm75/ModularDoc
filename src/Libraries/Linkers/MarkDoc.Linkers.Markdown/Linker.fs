namespace MarkDoc.Linkers.Markdown

open MarkDoc.Members
open MarkDoc.Linkers
open MarkDoc.Members.ResolvedTypes
open MarkDoc.Members.Types
open MarkDoc.Members.Members
open System.Collections.Generic
open System.Collections.Concurrent
open System.Text.RegularExpressions

type Linker(memberResolver) =
  let m_memberResolver : IResolver = memberResolver
  let m_anchors = new ConcurrentDictionary<IMember, Lazy<string>>()

  static let normalizerRegex = new Regex(@"(?<wh>\s)|(?<sym>[^A-Za-z0-9]*)")
  static let normalizerDictionary (x: Match) =
    match x.Groups |> Seq.tryFind (fun x -> x.Success) with
    | Some as s ->
      match s.Value.Name with 
      | "wh" -> "-"
      | "sym" -> ""
      | _ -> x.Value
    | None -> x.Value

  let generateStructure =
    let getName (input: IType) = 
      match input with
      | :? IInterface as i ->
        i.Name + new System.String('T', i.Generics.Count)
      | _ ->
        input.Name

    let result = new Dictionary<IType, string>()
    m_memberResolver.Types.Value
    |> Seq.map (fun x -> x.Value |> Seq.map (fun y -> (y, x.Key.ToLowerInvariant().Replace('.', '/') + "/" + getName y)))
    |> Seq.collect id
    |> Seq.iter (fun x -> result.Add(fst x, snd x))

    result :> IReadOnlyDictionary<IType, string>

  let createLink (source: IType, target: IType) =
    let mutable result : string = null
    if generateStructure.TryGetValue(target, &result) then
      result
    else
      ""

  let createResLink (source: IType, target: IResType) =
    if isNull target.Reference.Value then
      ""
    else
      createLink(source, target.Reference.Value)

  let createAnchor (input: IMember) =
    let normalizeAnchor (anchor: string) =
      normalizerRegex.Replace(anchor.ToLowerInvariant(), normalizerDictionary)

    let mutable result : Lazy<string> = null
    if m_anchors.TryGetValue(input, &result) then
      result.Value |> normalizeAnchor
    else
      ""

  let registerAnchor(target: IMember, anchor: Lazy<string>) =
    m_anchors.TryAdd(target, anchor) |> ignore

  interface ILinker with
    member __.Paths with get() = generateStructure

    member __.CreateLink(source: IType inref, target: IType inref) = createLink(source, target)
    member __.CreateLink(source: IType inref, target: IResType inref) = createResLink(source, target)
    member __.RegisterAnchor(target: IMember, anchor: Lazy<string>) = registerAnchor(target, anchor) |> ignore
    member __.CreateAnchor(target: IMember) = createAnchor target
