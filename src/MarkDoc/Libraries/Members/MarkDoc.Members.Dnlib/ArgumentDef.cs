using MarkDoc.Members.Enums;
using System;
using System.Diagnostics;
using System.Threading;

namespace MarkDoc.Members.Dnlib
{
  [DebuggerDisplay(nameof(ArgumentDef) + ": {Name}")]
  public class ArgumentDef
    : IArgument
  {
    #region Properties

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
    internal ArgumentDef(dnlib.DotNet.Parameter source)
    {
      if (source == null)
        throw new ArgumentNullException(nameof(source));

      Name = source.Name;
      Keyword = ResolveKeyword(source);
      Type = ResolveType(source);
    }

    #region Methods

    private static IResType ResolveType(dnlib.DotNet.Parameter source)
      => Resolver.Instance.Resolve(source.Type);

    private static ArgumentType ResolveKeyword(dnlib.DotNet.Parameter source)
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
