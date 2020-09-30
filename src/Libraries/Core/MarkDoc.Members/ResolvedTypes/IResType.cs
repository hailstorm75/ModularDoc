using System;
using MarkDoc.Members.Types;

namespace MarkDoc.Members.ResolvedTypes
{
  /// <summary>
  /// Interface for resolved types
  /// </summary>
  public interface IResType
  {
    #region Properties

    /// <summary>
    /// Resolved type display name
    /// </summary>
    string DisplayName { get; }

    /// <summary>
    /// Resolved type name for documentation
    /// </summary>
    string DocumentationName { get; }

    /// <summary>
    /// Resolved type raw name
    /// </summary>
    string RawName { get; }

    /// <summary>
    /// Resolved type namespace
    /// </summary>
    string TypeNamespace { get; }

    /// <summary>
    /// Reference to known type
    /// </summary>
    Lazy<IType?> Reference { get; }

    /// <summary>
    /// Is the type a reference
    /// </summary>
    bool IsByRef { get;  }

    #endregion
  }
}
