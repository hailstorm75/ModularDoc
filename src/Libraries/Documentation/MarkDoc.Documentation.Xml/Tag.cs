using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using MarkDoc.Documentation.Tags;
using static MarkDoc.Documentation.ITag;

namespace MarkDoc.Documentation.Xml
{
  public class Tag
    : ITag
  {
    #region Properties

    /// <inheritdoc />
    public TagType Type { get; }

    /// <inheritdoc />
    public string Reference { get; }

    /// <inheritdoc />
    public IReadOnlyCollection<IContent> Content { get; }

    #endregion

    public Tag(XElement source)
    {
      static TagType ResolveType(XElement source)
        => source.Name.LocalName.ToUpperInvariant() switch
        {
          "SUMMARY" => TagType.Summary,
          "REMARKS" => TagType.Remarks,
          "EXAMPLE" => TagType.Example,
          "RETURNS" => TagType.Returns,
          "VALUE" => TagType.Value,
          "EXCEPTION" => TagType.Exception,
          "PARAM" => TagType.Param,
          "TYPEPARAM" => TagType.Typeparam,
          "SEEALSO" => TagType.Seealso,
          "INHERITDOC" => TagType.Inheritdoc,
          _ => TagType.InvalidTag
        };

      static string ResolveReference(TagType type, XElement source)
      {
        switch (type)
        {
          case TagType.Inheritdoc:
          case TagType.Exception:
          case TagType.Seealso:
            return source.Attributes().FirstOrDefault(x => x.Name.LocalName.Equals("cref", StringComparison.InvariantCultureIgnoreCase))?.Value ?? string.Empty;
          case TagType.Param:
          case TagType.Typeparam:
            return source.Attributes().First(x => x.Name.LocalName.Equals("name", StringComparison.InvariantCultureIgnoreCase)).Value;
          default:
            return string.Empty;
        }
      }

      if (source == null)
        throw new ArgumentNullException(nameof(source));

      Type = ResolveType(source);
      Reference = ResolveReference(Type, source);
      Content = source.Nodes()
        .Select(ContentResolver.Resolve)
        .Where(x => x != null)
        .Cast<IContent>()
        .ToArray();
    }
  }
}
