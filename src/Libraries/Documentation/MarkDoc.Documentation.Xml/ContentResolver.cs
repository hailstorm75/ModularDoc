using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using MarkDoc.Documentation.Tags;
using MarkDoc.Documentation.Xml.Tags;

namespace MarkDoc.Documentation.Xml
{
  internal static class ContentResolver
  {
    public static IEnumerable<IContent> Resolve(XNode node)
      => node switch
      {
        XText element => new[] { new TextTag(element) },
        XElement element => element.Name.LocalName.ToUpperInvariant() switch
        {
          "LIST" => new[] { new ListTag(element) },
          _ => ResolveInnerTag(element)
        },
        _ => Enumerable.Empty<IContent>()
      };

    private static IEnumerable<IContent> ResolveInnerTag(XElement element)
    {
      var result = new InnerTag(element);
      if (result.Type == IInnerTag.InnerTagType.InvalidTag)
        return result.Content;
      else
        return new[] { result };
    }
  }
}
