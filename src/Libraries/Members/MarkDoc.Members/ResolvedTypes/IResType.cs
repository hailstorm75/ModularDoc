using System;

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
    /// Resolved type name
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Resolved type namespace
    /// </summary>
    string TypeNamespace { get; }

    /// <summary>
    /// Reference to known type
    /// </summary>
    Lazy<IType?> Reference { get; } 

    #endregion
  }
}
