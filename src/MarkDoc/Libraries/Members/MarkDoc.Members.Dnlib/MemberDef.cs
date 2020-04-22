using dnlib.DotNet;
using MarkDoc.Members.Enums;
using System;

namespace MarkDoc.Members.Dnlib
{
  public abstract class MemberDef
    : IMember
  {
    #region Properties

    /// <inheritdoc />
    public bool IsObsolete { get; }

    /// <inheritdoc />
    public abstract bool IsStatic { get; }

    /// <inheritdoc />
    public string Name { get; }

    /// <inheritdoc />
    public AccessorType Accessor { get; }

    #endregion

    /// <summary>
    /// Default constructor
    /// </summary>
    protected MemberDef(dnlib.DotNet.IMemberDef source)
    {
      if (source == null)
        throw new ArgumentNullException(nameof(source));

      Name = ResolveName(source);
    }

    private static string ResolveName(dnlib.DotNet.IMemberDef source)
      => source.FullName;
  }
}
