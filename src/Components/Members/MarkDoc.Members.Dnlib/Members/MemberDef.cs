using System;
using MarkDoc.Members.Enums;
using MarkDoc.Members.Members;

namespace MarkDoc.Members.Dnlib.Members
{
  /// <summary>
  /// Base class for members
  /// </summary>
  public abstract class MemberDef
    : IMember
  {
    #region Properties

    /// <summary>
    /// Type resolver
    /// </summary>
    protected Resolver Resolver { get; }

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
    protected internal MemberDef(Resolver resolver, dnlib.DotNet.IMemberDef source)
    {
      // If the source is null..
      if (source is null)
        // throw an exception
        throw new ArgumentNullException(nameof(source));

      Resolver = resolver;
    }
  }
}
