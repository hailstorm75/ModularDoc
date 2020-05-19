using dnlib.DotNet;
using MarkDoc.Members.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using MarkDoc.Members.ResolvedTypes;

namespace MarkDoc.Members.Dnlib
{
  [DebuggerDisplay(nameof(ArgumentDef) + ": {Name}")]
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
    internal ArgumentDef(IResolver resolver, dnlib.DotNet.Parameter source, IReadOnlyDictionary<string, string> generics)
    {
      if (source == null)
        throw new ArgumentNullException(nameof(source));
      if (generics == null)
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

      // TODO: Implement ref support

      if (def.IsIn)
        return ArgumentType.In;
      if (def.IsOut)
        return ArgumentType.Out;
      if (def.IsOptional)
        return ArgumentType.Optional;

      return ArgumentType.Normal;
    } 

    #endregion
  }
}
