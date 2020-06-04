namespace MarkDoc.Generator.Basic
open MarkDoc.Members.Enums
open MarkDoc.Members
open MarkDoc.Elements
open System

module internal Helpers =

  let partial f x y = f(x , y)

  let toElement x = x :> IElement 

  let argumeentTypeStr (arg : IArgument) =
    match arg.Keyword with
    | ArgumentType.In -> "in"
    | ArgumentType.Out -> "out"
    | ArgumentType.Ref -> "ref"
    | _ -> ""

  let staticStr (x : bool) =
    match x with
    | true -> "Static"
    | false -> ""

  let accessorStr (accessor : AccessorType) =
    match accessor with
    | AccessorType.Public -> "Public"
    | AccessorType.Protected -> "Protected"
    | AccessorType.Internal -> "Internal"
    | _ -> ""

  let methodArguments (item : IMethod) =
    let argument (arg : IArgument) =
      seq [ argumeentTypeStr arg; arg.Type.DisplayName; arg.Name ]
      |> partial String.Join " "

    item.Arguments
    |> Seq.map argument
    |> partial String.Join ", "

  let groupMembers (members : seq<'M> when 'M :> IMember) =
    let byStatic(x : 'M) = not x.IsStatic
    let byAccessor (x : bool * seq<'M>) = (fst x, snd x |> Seq.groupBy(fun y -> y.Accessor))

    members
    |> Seq.groupBy byStatic
    |> Seq.map byAccessor

  let flatten item =
    let a = fst item
    snd item
    |> Seq.map(fun x -> (a, fst x, snd x))