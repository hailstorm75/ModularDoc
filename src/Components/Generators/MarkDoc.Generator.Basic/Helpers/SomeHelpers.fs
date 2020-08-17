namespace MarkDoc.Generator.Basic

module internal SomeHelpers =
  let whereSome input =
    input |> Option.isSome
  let whereSome2 input =
    input |> fst |> whereSome
  let get2 input: 'c * 'd =
    let get f: 'g =
      let v: obj = input |> f
      match v with
      | :? Option<'g> as a -> Option.get a
      | _ -> v :?> 'g

    (get fst, get snd)

