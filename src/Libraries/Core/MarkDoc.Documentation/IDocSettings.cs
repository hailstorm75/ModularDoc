using System.Collections.Generic;

namespace MarkDoc.Documentation
{
  /// <summary>
  /// Interface for <see cref="IDocResolver"/> settings
  /// </summary>
  public interface IDocSettings
  {
    /// <summary>
    /// Paths to documentation sources
    /// </summary>
    IReadOnlyCollection<string> Paths { get; set; }
  }
}