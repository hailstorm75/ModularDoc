using System;
using ModularDoc.Members.Enums;
using ModularDoc.Members.Members;

namespace ModularDoc.Members.Dnlib.Members
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

    /// <inheritdoc />
    public abstract string RawName { get; }

    /// <inheritdoc />
    public (int line, string source)? LineSource { get; }

    #endregion

    /// <summary>
    /// Default constructor
    /// </summary>
    // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
    protected internal MemberDef(Resolver resolver, dnlib.DotNet.IMemberDef source)
    {
      // If the source is null..
      if (source is null)
        // throw an exception
        throw new ArgumentNullException(nameof(source));

      Resolver = resolver;

      if (Resolver.TryGetMemberSourceLine((int)source.MDToken.Raw, out var line, out var lineSource))
        LineSource = (line, lineSource);
    }
  }
}
