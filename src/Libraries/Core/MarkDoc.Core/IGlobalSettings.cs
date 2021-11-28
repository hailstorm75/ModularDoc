using System.Collections.Generic;

namespace MarkDoc.Core
{
  /// <summary>
  /// Interface for global settings
  /// </summary>
  public interface IGlobalSettings
    : ILibrarySettings
  {
    /// <summary>
    /// Globally ignored namespaces
    /// </summary>
    IReadOnlyCollection<string> IgnoredNamespaces { get; }

    /// <summary>
    /// Globally ignored types
    /// </summary>
    IReadOnlyCollection<string> IgnoredTypes { get; }

    /// <summary>
    /// Globally ignored but previously selected namespaces
    /// </summary>
    IReadOnlyCollection<string> CheckedIgnoredNamespaces { get; }

    /// <summary>
    /// Globally ignored but previously selected types
    /// </summary>
    IReadOnlyCollection<string> CheckedIgnoredTypes { get; }
  }
}