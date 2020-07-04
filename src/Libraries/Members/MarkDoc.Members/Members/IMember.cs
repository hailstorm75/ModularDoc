using System;
using MarkDoc.Members.Enums;

namespace MarkDoc.Members.Members
{
  /// <summary>
  /// Interface for type members
  /// </summary>
  public interface IMember
    : IEquatable<IMember>
  {
    #region Properties

    /// <summary>
    /// Is method static
    /// </summary>
    bool IsStatic { get; }

    /// <summary>
    /// Member name
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Raw member name
    /// </summary>
    string RawName { get; }

    /// <summary>
    /// Member accessor
    /// </summary>
    AccessorType Accessor { get; } 

    #endregion

#pragma warning disable CA1033 // Interface methods should be callable by child types
    bool IEquatable<IMember>.Equals(IMember other)
    {
      if (ReferenceEquals(this, other))
        return true;
      if (IsStatic != other?.IsStatic)
        return false;

      return RawName.Equals(other?.RawName ?? string.Empty, StringComparison.InvariantCulture);
    }
#pragma warning restore CA1033 // Interface methods should be callable by child types
  }
}
