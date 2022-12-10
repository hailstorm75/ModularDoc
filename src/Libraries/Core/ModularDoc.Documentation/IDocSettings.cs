using System.Collections.Generic;
using ModularDoc;
using ModularDoc.Core;

namespace ModularDoc.Documentation
{
  /// <summary>
  /// Interface for <see cref="IDocResolver"/> settings
  /// </summary>
  public interface IDocSettings
    : ILibrarySettings
  {
    /// <summary>
    /// Paths to documentation sources
    /// </summary>
    IReadOnlyCollection<string> Paths { get; set; }
  }
}