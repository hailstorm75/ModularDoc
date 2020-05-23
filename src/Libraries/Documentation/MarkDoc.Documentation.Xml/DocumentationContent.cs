using MarkDoc.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using MarkDoc.Documentation.Tags;
using MarkDoc.Documentation.Xml.Tags;
using static MarkDoc.Documentation.Tags.ITag;

namespace MarkDoc.Documentation.Xml
{
  public class DocumentationContent
    : IDocumentation
  {
    #region Properties

    /// <inheritdoc />
    public IReadOnlyDictionary<TagType, IReadOnlyCollection<ITag>> Tags { get; }

    /// <inheritdoc />
    public bool HasInheritDoc { get; }

    /// <inheritdoc />
    public string InheritDocRef { get; }

    #endregion

    public DocumentationContent(XElement source)
    {
      if (source == null)
        throw new ArgumentNullException(nameof(source));

      Tags = ResolveTags(source.Elements())
        .Where(x => x.Type != TagType.InvalidTag)
        .GroupBy(x => x.Type)
        .ToDictionary(Linq.GroupKey, x => x.GroupValues().ToReadOnlyCollection());

      HasInheritDoc = Tags.TryGetValue(TagType.Inheritdoc, out var tags);
      InheritDocRef = tags?.First()?.Reference ?? string.Empty;
    }

    public DocumentationContent(IReadOnlyDictionary<TagType, IReadOnlyCollection<ITag>> tags)
    {
      if (tags == null)
        throw new ArgumentNullException(nameof(tags));

      Tags = tags;
      HasInheritDoc = Tags.TryGetValue(TagType.Inheritdoc, out var t);
      InheritDocRef = t?.First()?.Reference ?? string.Empty;
    }

    private static IEnumerable<ITag> ResolveTags(IEnumerable<XElement> source)
    {
      foreach (var node in source)
        yield return new Tag(node);
    }
  }
}
