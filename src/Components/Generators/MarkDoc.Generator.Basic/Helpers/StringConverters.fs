namespace MarkDoc.Generator.Basic

open MarkDoc.Members.Members
open MarkDoc.Members.Enums

module StringConverters =
  let toLower(x : string) =
    x.ToLower()

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

  let inheritanceStr (inheritance : MemberInheritance) =
    match inheritance with
    | MemberInheritance.Abstract -> "abstract"
    | MemberInheritance.Override -> "override"
    | MemberInheritance.Virtual -> "virtual"
    | _ -> ""
