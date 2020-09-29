using System;
using System.Collections.Generic;
using System.Diagnostics;
using dnlib.DotNet;
using MarkDoc.Members.Enums;
using MarkDoc.Members.Members;
using MarkDoc.Members.ResolvedTypes;

namespace MarkDoc.Members.Dnlib.Members
{
  /// <summary>
  /// Class for representing arguments
  /// </summary>
  [DebuggerDisplay(nameof(ArgumentDef) + (": {" + nameof(Name) + "}"))]
  public class ArgumentDef
    : IArgument
  {
    #region Properties

    protected IResolver Resolver { get; }

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
    internal ArgumentDef(IResolver resolver, Parameter source, IReadOnlyDictionary<string, string> generics)
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
      => Resolver.Resolve(source.Type, generics);

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

      return ArgumentType.Normal;
    }

    #endregion
  }
}
