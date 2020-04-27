using MarkDoc.Members.Enums;
using System;

namespace MarkDoc.Members.Dnlib
{
  public abstract class MemberDef
    : IMember
  {
    #region Properties

    /// <inheritdoc />
    public abstract bool IsStatic { get; }

    /// <inheritdoc />
    public abstract string Name { get; }

    /// <inheritdoc />
    public abstract AccessorType Accessor { get; }

    #endregion

    /// <summary>
    /// Default constructor
    /// </summary>
    protected MemberDef(dnlib.DotNet.IMemberDef source)
    {
      if (source == null)
        throw new ArgumentNullException(nameof(source));
    }
  }
}
