namespace MarkDoc.Generator.Basic

open MarkDoc.Members.Members
open MarkDoc.Members.Enums

module internal StringConverters =
  /// <summary>
  /// Returns a copy of this <paramref name="text"/> converted to lowercase
  /// </summary>
  /// <param name="text">Input text</param>
  /// <returns>Text converted to lowercase</returns>
  let toLower (text: string) = text.ToLower()

  /// <summary>
  /// Returns a string representation of the input <paramref name="argument"/>
  /// </summary>
  /// <param name="argument">Input argument</param>
  /// <returns>String representation of the input <paramref name="argument"/></returns>
  let argumentTypeStr (argument: IArgument) =
    // Retuns the respective string representation of the argument type
    match argument.Keyword with
    | ArgumentType.In -> "in"
    | ArgumentType.Out -> "out"
    | ArgumentType.Ref -> "ref"
    | _ -> ""

  /// <summary>
  /// Returns a string representation of whether a member is static or not
  /// </summary>
  /// <param name="isStatic">Determines whether a given member is static or not</param>
  /// <returns>String representation of whether a member is static or not</returns>
  let staticStr (isStatic: bool) =
    match isStatic with
    | true -> "Static"
    | false -> ""

  /// <summary>
  /// Returns a string representation of the input <paramref name="variance"/>
  /// </summary>
  /// <param name="variance">Input variance</param>
  /// <returns>String representation of the input <paramref name="variance"/></returns>
  let varianceStr (variance: Variance) =
    // Retuns the respective string representation of the variance type
    match variance with
    | Variance.Contravariant -> "in"
    | Variance.Covariant -> "out"
    | _ -> ""

  /// <summary>
  /// Returns a string representation of the input <paramref name="accessor"/>
  /// </summary>
  /// <param name="accessor">Input accessor</param>
  /// <returns>String representation of the input <paramref name="accessor"/></returns>
  let accessorStr (accessor: AccessorType) =
    // Retuns the respective string representation of the accessor type
    match accessor with
    | AccessorType.Public -> "Public"
    | AccessorType.Protected -> "Protected"
    | AccessorType.Internal -> "Internal"
    | AccessorType.ProtectedInternal -> "Protected internal"
    | _ -> ""

  /// <summary>
  /// Returns a string representation of the input <paramref name="inheritance"/>
  /// </summary>
  /// <param name="inheritance">Input inheritance</param>
  /// <returns>String representation of the input <paramref name="inheritance"/></returns>
  let inheritanceStr (inheritance: MemberInheritance) =
    // Retuns the respective string representation of the inheritance type
    match inheritance with
    | MemberInheritance.Abstract -> "abstract"
    | MemberInheritance.Override -> "override"
    | MemberInheritance.Virtual -> "virtual"
    | _ -> ""
