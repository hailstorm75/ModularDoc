using System.Xml.Linq;
using MarkDoc.Documentation.Tags;

namespace MarkDoc.Documentation.Xml
{
  internal static class ContentResolver
  {
    public static IContent? Resolve(XNode node)
      => node switch
      {
        XText element => new TextTag(element),
        XElement element => element.Name.LocalName.ToUpperInvariant() switch
        {
          "LIST" => new ListTag(element),
          _ => new InnerTag(element)
        },
        _ => null
      };
  }
}
