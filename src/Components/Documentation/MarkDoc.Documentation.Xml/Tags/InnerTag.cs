using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using MarkDoc.Documentation.Tags;
using MarkDoc.Helpers;

namespace MarkDoc.Documentation.Xml.Tags
{
  /// <summary>
  /// Documentation tag which can be within other tags
  /// </summary>
  public class InnerTag
    : IInnerTag
  {
    #region Properties

    /// <inheritdoc />
    public IInnerTag.InnerTagType Type { get; }

    /// <inheritdoc />
    public string Reference { get; }

    /// <inheritdoc />
    public IReadOnlyCollection<IContent> Content { get; }

    #endregion

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="source">Documentation source</param>
    internal InnerTag(XElement source)
    {
      // If the source is null..
      if (source is null)
        // throw an exception
        throw new ArgumentNullException(nameof(source));

      Type = ResolveType(source);
      Reference = ResolveReference(Type, source);
      Content = source
        // Select the documentation tags
        .Nodes()
        // Resolve the tags
        .Select(ContentResolver.Resolve)
        // Flatten the sequence
        .SelectMany(Linq.XtoX)
        // Materialize to a collection
        .ToReadOnlyCollection();
    }

    #region Methods

    private static IInnerTag.InnerTagType ResolveType(XElement source)
      => source.Name.LocalName.ToUpperInvariant() switch
      {
        "SEE" => IInnerTag.InnerTagType.See,
        "C" => IInnerTag.InnerTagType.CodeSingle,
        "CODE" => IInnerTag.InnerTagType.Code,
        "PARA" => IInnerTag.InnerTagType.Para,
        "PARAMREF" => IInnerTag.InnerTagType.ParamRef,
        "TYPEPARAMREF" => IInnerTag.InnerTagType.TypeRef,
        _ => IInnerTag.InnerTagType.InvalidTag
      };

    private static string ResolveReference(IInnerTag.InnerTagType type, XElement source)
    {
      switch (type)
      {
        case IInnerTag.InnerTagType.ParamRef:
        case IInnerTag.InnerTagType.TypeRef:
          return source.Attributes().First(x => x.Name.LocalName.Equals("name", StringComparison.InvariantCultureIgnoreCase)).Value;
        case IInnerTag.InnerTagType.See:
        case IInnerTag.InnerTagType.SeeAlso:
          return source.Attributes().FirstOrDefault(x => x.Name.LocalName.Equals("cref", StringComparison.InvariantCultureIgnoreCase))?.Value ?? string.Empty;
        default:
          return string.Empty;
      }
    }

    #endregion
  }
}
