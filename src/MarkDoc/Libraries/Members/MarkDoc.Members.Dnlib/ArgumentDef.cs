using MarkDoc.Members.Enums;
using System;

namespace MarkDoc.Members.Dnlib
{
  public class ArgumentDef
    : IArgument
  {
    #region Properties

    /// <inheritdoc />
    public ArgumentType Keyword { get; }

    /// <inheritdoc />
    public Lazy<IType> Type { get; }

    #endregion

    /// <summary>
    /// Default constructor
    /// </summary>
    public ArgumentDef(dnlib.DotNet.Parameter source)
    {

    }
  }
}
