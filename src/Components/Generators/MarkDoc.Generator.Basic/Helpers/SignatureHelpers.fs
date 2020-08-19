namespace MarkDoc.Generator.Basic

open MarkDoc.Members.Members
open System
open MarkDoc.Members.Enums

module internal SignatureHelpers =
  type Processor = IMember -> string

  let getGenerics (input: IMember) = 
    let generics = match input with
                   | :? IMethod as method -> method.Generics :> string seq
                   | :? IDelegate as del -> del.Generics :> string seq
                   | _ -> Seq.empty

    if Seq.isEmpty generics then
      ""
    else
      String.Format("<{0}>", String.Join(", ", generics))
  let getAccessor (input: IMember) =
    input.Accessor |> StringConverters.accessorStr |> StringConverters.toLower
  let getOperator (input: IMember) =
    match input with
    | :? IMethod as method ->
      if method.Operator <> OperatorType.None then " operator" else ""
    | _ -> ""
  let getName (input: IMember) =
    input.Name
  let getStatic (input: IMember) =
    if input.IsStatic then " static" else ""
  let getReturn (input: IMember) =
    match input with
    | :? IMethod as method ->
      match method.Operator with
      | OperatorType.Implicit -> "implicit"
      | OperatorType.Explicit -> "explicit"
      | OperatorType.None
      | OperatorType.Normal
      | _ -> if isNull method.Returns then "void" else method.Returns.DisplayName
    | :? IDelegate as deleg->
      if isNull deleg.Returns then "void" else deleg.Returns.DisplayName
    | :? IEvent as ev ->
      ev.Type.DisplayName
    | :? IProperty as prop ->
      prop.Type.DisplayName
    | _ -> ""
  let getAsync (input: IMember) =
    match input with
    | :? IMethod as method ->
      if method.IsAsync then "async " else ""
    | _ -> ""
  let getInheritance (input: IMember) =
    let processInheritance inheritance =
      if (inheritance = MemberInheritance.Normal) then "" else " " + (StringConverters.inheritanceStr inheritance)

    match input with
    | :? IMethod as method -> method.Inheritance |> processInheritance
    | :? IProperty as property -> property.Inheritance |> processInheritance
    | _ -> ""
  let getPropertyMethods (input: IMember) =
    match input with
    | :? IProperty as property ->
      let accessor acc = 
        match acc with
        | AccessorType.Protected -> if property.Accessor.Equals acc then "" else "protected "
        | AccessorType.Internal -> if property.Accessor.Equals acc then "" else "internal "
        | _ -> ""
      String.Join("; ", seq [
        if property.GetAccessor.HasValue then
          yield (accessor property.GetAccessor.Value) + "get" 
        if property.SetAccessor.HasValue then
          yield (accessor property.SetAccessor.Value) + "set" 
      ])
    | _ -> ""

  let generateSignature format (processors: Processor seq) input =
    let x input (col: obj[]) =
      String.Format(input, col)

    x format (processors |> Seq.map(fun x -> x input :> obj) |> Seq.toArray) |> Code

