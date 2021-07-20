using System.Collections.Generic;

namespace MarkDoc.Members
{
  /// <summary>
  /// Interface for <see cref="IResolver"/> settings
  /// </summary>
  public interface IMemberSettings
  {
    /// <summary>
    /// Paths to member sources
    /// </summary>
    IReadOnlyCollection<string> Paths { get; }
  }
}