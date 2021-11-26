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
    public const string ENTRY_PATHS = "assemblyPaths";
    public const string PATH_DELIMITER = "|";

    /// <summary>
    /// Paths to member sources
    /// </summary>
    IReadOnlyCollection<string> Paths { get; }
  }
}