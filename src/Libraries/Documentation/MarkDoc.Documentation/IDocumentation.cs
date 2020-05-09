using System.Collections.Generic;
using static MarkDoc.Documentation.ITag;

namespace MarkDoc.Documentation
{
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
  }
}
