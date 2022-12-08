using ModularDoc.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using ModularDoc.Documentation.Tags;
using ModularDoc.Documentation.Xml.Tags;
using static ModularDoc.Documentation.Tags.ITag;

namespace ModularDoc.Documentation.Xml
{
  /// <summary>
  /// Documentation container
  /// </summary>
  public readonly struct DocumentationContent
    : IDocumentation, IEquatable<DocumentationContent>
  {
    #region Properties

    /// <inheritdoc />
    public IReadOnlyDictionary<TagType, IReadOnlyCollection<ITag>> Tags { get; }

    /// <inheritdoc />
    public bool HasInheritDoc { get; }

    /// <inheritdoc />
    public string InheritDocRef { get; }

    #endregion

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="source">Documentation source</param>
    internal DocumentationContent(XElement source)
    {
      // If the source is null..
      if (source is null)
        // throw an exception
        throw new ArgumentNullException(nameof(source));

      Tags = ResolveTags(source.Elements())
        // Filter out invalid tags
        .Where(x => x.Type != TagType.InvalidTag)
        // Group the tags by their type
        .GroupBy(x => x.Type)
        // Materialize the group into a dictionary
        .ToDictionary(Linq.GroupKey, x => x.GroupValues().ToReadOnlyCollection());

      HasInheritDoc = Tags.TryGetValue(TagType.Inheritdoc, out var tags);
      InheritDocRef = tags?.First().Reference ?? string.Empty;
    }

    /// <summary>
    /// Hidden constructor
    /// </summary>
    /// <param name="tags">Explicit documentation source</param>
    internal DocumentationContent(IReadOnlyDictionary<TagType, IReadOnlyCollection<ITag>> tags)
    {
      Tags = tags ?? throw new ArgumentNullException(nameof(tags));
      HasInheritDoc = Tags.TryGetValue(TagType.Inheritdoc, out var t);
      InheritDocRef = t?.First().Reference ?? string.Empty;
    }

    #region Methods

    private static IEnumerable<ITag> ResolveTags(IEnumerable<XElement> source)
      => source.Select(node => new Tag(node));

    /// <inheritdoc />
    public override bool Equals(object? obj)
      => obj is DocumentationContent doc && Equals(doc);

    public static bool operator ==(DocumentationContent left, DocumentationContent right)
      => left.Equals(right);

    public static bool operator !=(DocumentationContent left, DocumentationContent right)
      => !(left == right);

    /// <inheritdoc />
    public override int GetHashCode()
      => HashCode.Combine(Tags, HasInheritDoc, InheritDocRef);

    /// <inheritdoc />
    public bool Equals(DocumentationContent other)
      => Tags.Equals(other.Tags) && HasInheritDoc == other.HasInheritDoc && InheritDocRef == other.InheritDocRef;

    #endregion
  }
}
