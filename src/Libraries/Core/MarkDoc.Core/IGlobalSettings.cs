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
    IReadOnlyCollection<string> IgnoredNamespaces { get; set; }

    /// <summary>
    /// Globally ignored types
    /// </summary>
    IReadOnlyCollection<string> IgnoredTypes { get; set; }
  }
}