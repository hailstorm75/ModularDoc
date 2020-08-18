namespace MarkDoc.Generator.Basic

module internal SomeHelpers =
  let emptyToNone input =
    if Seq.isEmpty input then
      None
    else
      input |> Some
  let whereSome input =
    input
    |> Seq.filter Option.isSome
    |> Seq.map Option.get
  let whereSome2 input =
    input
    |> Seq.filter (fst >> Option.isSome)
    |> Seq.map (fun x -> x |> fst |> Option.get, x |> snd)
  let get2 input: 'c * 'd =
    let get f: 'g =
      let v: obj = input |> f
      match v with
      | :? Option<'g> as a -> Option.get a
      | _ -> v :?> 'g

    (get fst, get snd)

