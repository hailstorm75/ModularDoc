namespace ModularDoc.Composer.Basic

module internal SomeHelpers =
  /// <summary>
  /// Returns <see cref="None"/> if the <paramref name="input"/> sequence is empty
  /// </summary>
  /// <param name="input">Input sequence</param>
  /// <returns>Optioned sequence</returns>
  let emptyToNone input =
    // if the sequence is empty..
    if Seq.isEmpty input then
      // return nothing
      None
    // Otherwise..
    else
      // return the sequence
      input |> Some

  /// <summary>
  /// Filters out items from the <paramref name="input"/> sequence which are <see cref="None"/>
  /// </summary>
  /// <param name="input">Input sequence of options</param>
  /// <returns>Filter sequence</returns>
  let whereSome input =
    input
    // Filter out items which are None
    |> Seq.filter Option.isSome
    // Extract the items wrapped in the options
    |> Seq.map Option.get

  /// <summary>
  /// Filters out tupled items from the <paramref name="input"/> sequence which have the first item equal to <see cref="None"/>
  /// </summary>
  /// <param name="input">Tuple sequence</param>
  /// <returns>Filter tuple sequence</returns>
  let whereSome2 input =
    input
    // Filter out tuple with the first element equal to None
    |> Seq.filter (fst >> Option.isSome)
    // Extract the items wrapped in the options and rewrap into a tuple
    |> Seq.map (fun x -> x |> fst |> Option.get, x |> snd)

