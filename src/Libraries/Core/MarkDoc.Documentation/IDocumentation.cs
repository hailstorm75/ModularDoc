using System.Collections.Generic;
using MarkDoc.Documentation.Tags;
using static MarkDoc.Documentation.Tags.ITag;

namespace MarkDoc.Documentation
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
