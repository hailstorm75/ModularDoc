using MarkDoc.Documentation.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace MarkDoc.Documentation.Xml
{
  public class ListTag
    : IListTag
  {
    #region Properties

    public IListTag.ListType Type { get; }

    public IReadOnlyCollection<IContent> Headings { get; }

    public IReadOnlyCollection<IReadOnlyCollection<IContent>> Rows { get; } 

    #endregion

    public ListTag(XElement node)
    {
      if (node == null)
        throw new ArgumentNullException(nameof(node));

      var rows = new LinkedList<IReadOnlyCollection<IContent>>();
      var headings = new LinkedList<IContent>();
      foreach (var tag in node.Nodes().OfType<XElement>().GroupBy(x => x.Name.LocalName.ToUpperInvariant()))
      {
        switch (tag.Key)
        {
          case "ITEM":
            ResolveRows(ref rows, tag.Select(x => x));
            break;
          case "LISTHEADER":
            ResolveHeadings(ref headings, tag.Select(x => x));
            break;
          default:
            continue;
        }
      }

      Type = ResolveType(node);
      Rows = rows;
      Headings = headings;
    }

    #region Methods

    private static IListTag.ListType ResolveType(XElement node)
    {
      var type = node.Attributes()
        .FirstOrDefault(x => x.Name.LocalName.Equals("type", StringComparison.InvariantCultureIgnoreCase))
        .Value;

      return type?.ToUpperInvariant() switch
      {
        "BULLET" => IListTag.ListType.Bullet,
        "NUMBER" => IListTag.ListType.Number,
        "TABLE" => IListTag.ListType.Table,
        _ => throw new NotSupportedException() // TODO: Exception message
      };
    }

    private static void ResolveHeadings(ref LinkedList<IContent> result, IEnumerable<XElement> elements)
    {
      foreach (var element in elements)
        result.AddLast(element.Nodes()
          .OfType<XElement>()
          .Select(x => x.FirstNode)
          .Select(ContentResolver.Resolve)
          .OfType<IContent>()
          .First());
    }

    private static void ResolveRows(ref LinkedList<IReadOnlyCollection<IContent>> result, IEnumerable<XElement> elements)
    {
      foreach (var element in elements)
        result.AddLast(element.Nodes()
          .OfType<XElement>()
          .Select(x => x.FirstNode)
          .Select(ContentResolver.Resolve)
          .OfType<IContent>()
          .ToArray());
    } 

    #endregion
  }
}
