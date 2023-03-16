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
    public const string ENTRY_PROCESS_PRIVATE = "processPrivate";
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

    /// <summary>
    /// Determines whether private types and members are processed
    /// </summary>
    bool ProcessPrivate { get; }
  }
}