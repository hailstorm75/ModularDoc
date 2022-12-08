using System.Collections.Generic;
using ModularDoc.Documentation.Tags;
using static ModularDoc.Documentation.Tags.ITag;

namespace ModularDoc.Documentation
{
  /// <summary>
  /// Interface for documentation containers
  /// </summary>
  public interface IDocumentation
  {
    /// <summary>
    /// Documentation tags
    /// </summary>
    IReadOnlyDictionary<TagType, IReadOnlyCollection<ITag>> Tags { get; }

    /// <summary>
    /// Contains the <c>inheritdoc</c> tag
    /// </summary>
    bool HasInheritDoc { get; }

    /// <summary>
    /// InheritDoc reference
    /// </summary>
    string InheritDocRef { get; }
  }
}
