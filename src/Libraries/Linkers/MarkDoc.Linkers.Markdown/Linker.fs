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

  let createLink (input: IType) =
    let mutable result : string = null
    if generateStructure.TryGetValue(input, &result) then
      result
    else
      ""

  let createResLink (input: IResType) =
    if isNull input.Reference.Value then
      ""
    else
      createLink(input.Reference.Value)

  let createAnchor (input: IMember) =
    ""

  interface ILinker with
    member __.Paths with get() = generateStructure

    member __.CreateLink(input: IType) = createLink input
    member __.CreateLink(input: IResType) = createResLink input
    member __.CreateAnchor(input: IMember) = createAnchor input
