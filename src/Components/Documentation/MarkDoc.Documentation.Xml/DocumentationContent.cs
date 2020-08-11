﻿using MarkDoc.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using MarkDoc.Documentation.Tags;
using MarkDoc.Documentation.Xml.Tags;
using static MarkDoc.Documentation.Tags.ITag;

namespace MarkDoc.Documentation.Xml
{
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

    public DocumentationContent(XElement source)
    {
      if (source is null)
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
      Tags = tags ?? throw new ArgumentNullException(nameof(tags));
      HasInheritDoc = Tags.TryGetValue(TagType.Inheritdoc, out var t);
      InheritDocRef = t?.First()?.Reference ?? string.Empty;
    }

    #region Methods

    private static IEnumerable<ITag> ResolveTags(IEnumerable<XElement> source)
      => source.Select(node => new Tag(node));

    /// <inheritdoc />
    public override bool Equals(object obj)
    {
      if (!(obj is DocumentationContent doc))
        return false;

      return Equals(doc);
    }

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