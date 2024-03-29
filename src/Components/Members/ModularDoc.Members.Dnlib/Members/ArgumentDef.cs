﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using dnlib.DotNet;
using ModularDoc.Members.Enums;
using ModularDoc.Members.Members;
using ModularDoc.Members.ResolvedTypes;

namespace ModularDoc.Members.Dnlib.Members
{
  /// <summary>
  /// Class for representing arguments
  /// </summary>
  [DebuggerDisplay(nameof(ArgumentDef) + (": {" + nameof(Name) + "}"))]
  public class ArgumentDef
    : IArgument
  {
    #region Properties

    protected Resolver Resolver { get; }

    /// <inheritdoc />
    public ArgumentType Keyword { get; }

    /// <inheritdoc />
    public IResType Type { get; }

    /// <inheritdoc />
    public string Name { get; }

    #endregion

    /// <summary>
    /// Default constructor
    /// </summary>
    internal ArgumentDef(Resolver resolver, Parameter source, IReadOnlyDictionary<string, string> generics)
    {
      // If the source is null..
      if (source is null)
        // throw an exception
        throw new ArgumentNullException(nameof(source));
      // If the generics are null..
      if (generics is null)
        // throw an exception
        throw new ArgumentNullException(nameof(generics));

      Resolver = resolver;
      Name = source.Name;
      Keyword = ResolveKeyword(source);
      Type = ResolveType(source, generics);
    }

    #region Methods

    private IResType ResolveType(Parameter source, IReadOnlyDictionary<string, string> generics)
      => Resolver.Resolve(source.Type, generics, source.ParamDef);

    private static ArgumentType ResolveKeyword(Parameter source)
    {
      var def = source.ParamDef;

      if (def.IsIn)
        return ArgumentType.In;
      if (def.IsOut)
        return ArgumentType.Out;
      if (def.IsOptional)
        return ArgumentType.Optional;
      if (source.Type.ElementType == ElementType.ByRef)
        return ArgumentType.Ref;
      if (def.CustomAttributes.Any(param => param.TypeFullName.Equals("System.ParamArrayAttribute", StringComparison.InvariantCultureIgnoreCase)))
        return ArgumentType.Param;
      return ArgumentType.Normal;
    }

    #endregion
  }
}
