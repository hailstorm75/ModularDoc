using MarkDoc.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using static MarkDoc.Documentation.ITag;

namespace MarkDoc.Documentation.Xml
{
  public class DocumentationContent
    : IDocumentation
  {
    #region Properties

    /// <inheritdoc />
    public IReadOnlyDictionary<TagType, IReadOnlyCollection<ITag>> Tags { get; }

    /// <inheritdoc />
    public bool HasInheritDoc
      => Tags.ContainsKey(TagType.Inheritdoc);

    #endregion

    public DocumentationContent(XElement source)
    {
      if (source == null)
        throw new ArgumentNullException(nameof(source));

      Tags = ResolveTags(source.Elements())
        .Where(x => x.Type != TagType.InvalidTag)
        .GroupBy(x => x.Type)
        .ToDictionary(Linq.GroupKey, x => x.GroupValues().ToReadOnlyCollection());
    }

    private static IEnumerable<ITag> ResolveTags(IEnumerable<XElement> source)
    {
      foreach (var node in source)
        yield return new Tag(node);
    }
  }
}
