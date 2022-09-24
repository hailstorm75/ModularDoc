using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using MarkDoc.Documentation.Tags;
using MarkDoc.Documentation.Xml.Tags;

namespace MarkDoc.Documentation.Xml
{
  /// <summary>
  /// Helper methods for resolving documentation tags
  /// </summary>
  internal static class ContentResolver
  {
    /// <summary>
    /// Resolves tags based on the provided <paramref name="source"/>
    /// </summary>
    /// <param name="source">Documentation source</param>
    /// <returns>Resolved tags</returns>
    public static IEnumerable<IContent> Resolve(XNode source)
      => source switch
      {
        XText element when element.Parent!.Name.LocalName.Equals("CODE", StringComparison.OrdinalIgnoreCase)
          => new[] { new CodeTag(element) },
        // Text
        XText element => new[] { new TextTag(element) },
        // Complex
        XElement element => element.Name.LocalName.ToUpperInvariant() switch
        {
          "LIST" => new[] { new ListTag(element) },
          _ => ResolveInnerTag(element)
        },
        // Invalid
        _ => Enumerable.Empty<IContent>()
      };

    private static IEnumerable<IContent> ResolveInnerTag(XElement element)
    {
      var result = new InnerTag(element);
      return result.Type == IInnerTag.InnerTagType.InvalidTag
        ? result.Content
        : new[] { result };
    }
  }
}
