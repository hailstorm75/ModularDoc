namespace MarkDoc.Linkers.Markdown

open MarkDoc.Members.Types

module private TypeHelper =
  let getName (input: IType) = 
    match input with
    | :? IInterface as i ->
      i.Name + new System.String('T', i.Generics.Count)
    | _ ->
      input.Name

