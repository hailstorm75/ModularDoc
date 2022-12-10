using System.Collections.Generic;
using ModularDoc;
using ModularDoc.Core;

namespace ModularDoc.Members
{
  /// <summary>
  /// Interface for <see cref="IResolver"/> settings
  /// </summary>
  public interface IMemberSettings
    : ILibrarySettings
  {
    /// <summary>
    /// Configuration name for assembly paths
    /// </summary>
    public const string ENTRY_PATHS = "assemblyPaths";
    /// <summary>
    /// Delimiter for paths
    /// </summary>
    public const string PATH_DELIMITER = "|";

    /// <summary>
    /// Paths to member sources
    /// </summary>
    IReadOnlyCollection<string> Paths { get; }
  }
}