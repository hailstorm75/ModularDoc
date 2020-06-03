using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using MarkDoc.Documentation.Tags;
using MarkDoc.Helpers;

namespace MarkDoc.Documentation.Xml.Tags
{
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

    public InnerTag(XElement node)
    {
      if (node is null)
        throw new ArgumentNullException(nameof(node));

      Type = ResolveType(node);
      Reference = ResolveReference(Type, node);
      Content = node.Nodes()
        .Select(ContentResolver.Resolve)
        .SelectMany(Linq.XtoX)
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
