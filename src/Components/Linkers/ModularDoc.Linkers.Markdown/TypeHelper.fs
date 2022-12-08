namespace ModularDoc.Linkers.Markdown

open ModularDoc.Members.Types

module private TypeHelper =
  /// <summary>
  /// Retrieves the type name
  /// </summary>
  /// <param name="input">Type to process</param>
  /// <returns>Retrieved type name</returns>
  let getName (input: IType) = 
    match input with
    | :? IInterface as i ->
      i.Name + System.String('T', i.Generics.Count)
    | _ ->
      input.Name

