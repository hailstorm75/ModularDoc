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
    /// Settings constant for paths
    /// </summary>
    public const string ENTRY_PATHS = "memberPaths";
    /// <summary>
    /// Delimiter for multiple paths
    /// </summary>
    public const string PATH_DELIMITER = "|";

    /// <summary>
    /// Paths to member sources
    /// </summary>
    IReadOnlyCollection<string> Paths { get; }
  }
}