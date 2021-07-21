using System.Collections.Generic;
using MarkDoc.Core;

namespace MarkDoc.Members
{
  /// <summary>
  /// Interface for <see cref="IResolver"/> settings
  /// </summary>
  public interface IMemberSettings
    : ILibrarySettings
  {
    /// <summary>
    /// Paths to member sources
    /// </summary>
    IReadOnlyCollection<string> Paths { get; }
  }
}