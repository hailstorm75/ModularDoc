using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using ModularDoc.Documentation.Tags;
using ModularDoc.Helpers;

namespace ModularDoc.Documentation.Xml.Tags
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
    public string Link { get; }

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
      Link = Type is IInnerTag.InnerTagType.See or IInnerTag.InnerTagType.SeeAlso
        ? source.Attributes().FirstOrDefault(x => x.Name.LocalName.Equals("href", StringComparison.InvariantCultureIgnoreCase))?.Value ?? string.Empty
        : string.Empty;

      var rawContent = source
        // Select the documentation tags
        .Nodes()
        // Resolve the tags
        .Select(ContentResolver.Resolve)
        // Flatten the sequence
        .SelectMany(Linq.XtoX);

      Content = Type switch
      {
        IInnerTag.InnerTagType.Code => MergeContent(rawContent).ToReadOnlyCollection(),
        IInnerTag.InnerTagType.CodeSingle => MergeContent(rawContent).ToReadOnlyCollection(),
        _ => rawContent.ToReadOnlyCollection()
      };
    }

    #region Methods

    private static IEnumerable<IContent> MergeContent(IEnumerable<IContent> content)
    {
      var builder = new StringBuilder();
      foreach (var item in content)
      {
        switch (item)
        {
          case ITextTag text:
            builder.Append(text.Content);
            break;
          case IInnerTag inner:
            var innerContent = inner.Type switch
            {
              IInnerTag.InnerTagType.Code => (inner.Content.First() as ITextTag)?.Content,
              IInnerTag.InnerTagType.CodeSingle => (inner.Content.First() as ITextTag)?.Content,
              IInnerTag.InnerTagType.ParamRef => inner.Reference,
              IInnerTag.InnerTagType.TypeRef => inner.Reference,
              IInnerTag.InnerTagType.SeeAlso => inner.Reference,
              IInnerTag.InnerTagType.See => inner.Reference,
              _ => string.Empty
            };

            builder.Append(innerContent);
            break;
        }
      }

      yield return new TextTag(builder.ToString());
    }

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
