using System.Collections.Generic;
using MarkDoc.Helpers;

namespace MarkDoc.Documentation.Xml
{
  public struct DocSettings
    : IDocSettings
  {
    /// <inheritdoc />
    public IReadOnlyCollection<string> Paths { get; set; }

    /// <summary>
    /// Default constructor
    /// </summary>
    public DocSettings(IEnumerable<string> paths)
      // TODO: Validate paths
      => Paths = paths.ToReadOnlyCollection();
  }
}