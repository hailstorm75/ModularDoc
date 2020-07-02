namespace MarkDoc.Generator.Basic
open MarkDoc.Members.Enums
open MarkDoc.Elements
open System
open MarkDoc.Documentation.Tags
open MarkDoc.Members.Members

module internal Helpers =

  let partial f x y = f(x , y)

  let toElement x = x :> IElement 

  let emptyToNone (a : seq<'a>) =
    if (Seq.isEmpty a) then
      None
    else
      Some(a)

  let toLower(x : string) =
    x.ToLower()

  let whereSome x =
    x
    |> Seq.filter Option.isSome
    |> Seq.map Option.get

  let processMethods (property : IProperty) =
    seq [
      if property.GetAccessor.HasValue then
        yield "get" 
      if property.SetAccessor.HasValue then
        yield "set" 
    ]

  let listType (t : IListTag.ListType) =
    match t with
    | IListTag.ListType.Bullet -> IList.ListType.Dotted
    | IListTag.ListType.Number -> IList.ListType.Numbered
    | _ -> raise (Exception()) // TODO: Message

  let argumentTypeStr (arg : IArgument) =
    match arg.Keyword with
    | ArgumentType.In -> "in"
    | ArgumentType.Out -> "out"
    | ArgumentType.Ref -> "ref"
    | _ -> ""

  let staticStr (x : bool) =
    match x with
    | true -> "Static"
    | false -> ""

  let varianceStr (x : Variance) =
    match x with
    | Variance.Contravariant -> "in"
    | Variance.Covariant -> "out"
    | _ -> ""

  let accessorStr (accessor : AccessorType) =
    match accessor with
    | AccessorType.Public -> "Public"
    | AccessorType.Protected -> "Protected"
    | AccessorType.Internal -> "Internal"
    | _ -> ""

  let groupMembers (members : seq<'M> when 'M :> IMember) =
    let byStatic(x : 'M) = x.IsStatic
    let byAccessor (x : bool * seq<'M>) = (fst x, snd x |> Seq.groupBy(fun y -> y.Accessor))

    members
    |> Seq.groupBy byStatic
    |> Seq.map byAccessor

  let flatten item =
    let a = fst item
    snd item
    |> Seq.map(fun x -> (a, fst x, snd x))