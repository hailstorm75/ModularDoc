using MarkDoc.Members.Enums;
using System;

namespace MarkDoc.Members.Dnlib
{
  public abstract class MemberDef
    : IMember
  {
    #region Properties

    protected IResolver Resolver { get; }

    /// <inheritdoc />
    public abstract bool IsStatic { get; }

    /// <inheritdoc />
    public abstract string Name { get; }

    /// <inheritdoc />
    public abstract AccessorType Accessor { get; }

    public abstract string RawName { get; }

    #endregion

    /// <summary>
    /// Default constructor
    /// </summary>
    internal protected MemberDef(IResolver resolver, dnlib.DotNet.IMemberDef source)
    {
      if (source == null)
        throw new ArgumentNullException(nameof(source));

      Resolver = resolver;
    }
  }
}
