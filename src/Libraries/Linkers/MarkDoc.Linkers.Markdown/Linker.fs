namespace MarkDoc.Linkers.Markdown

open MarkDoc.Members
open MarkDoc.Linkers
open MarkDoc.Members.ResolvedTypes
open MarkDoc.Members.Types
open MarkDoc.Members.Members
open System.Collections.Generic

type Linker(memberResolver) =
  let m_memberResolver : IResolver = memberResolver

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
    ""

  interface ILinker with
    member __.Paths with get() = generateStructure

    member __.CreateLink(source: IType inref, target: IType inref) = createLink(source, target)
    member __.CreateLink(source: IType inref, target: IResType inref) = createResLink(source, target)
    member __.CreateAnchor(target: IMember) = createAnchor target
