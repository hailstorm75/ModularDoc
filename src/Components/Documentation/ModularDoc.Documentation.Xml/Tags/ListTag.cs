using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using ModularDoc.Documentation.Tags;
using ModularDoc.Helpers;

namespace ModularDoc.Documentation.Xml.Tags
{
  /// <summary>
  /// Documentation tag for lists
  /// </summary>
  public class ListTag
    : IListTag
  {
    #region Properties

    /// <inheritdoc />
    public IListTag.ListType Type { get; }

    /// <inheritdoc />
    public IReadOnlyCollection<IContent> Headings { get; }

    /// <inheritdoc />
    public IReadOnlyCollection<IReadOnlyCollection<IContent>> Rows { get; }

    #endregion

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="source">Documentation source</param>
    internal ListTag(XElement source)
    {
      // If the source is null..
      if (source is null)
        // throw an exception
        throw new ArgumentNullException(nameof(source));

      // Prepare the rows collection
      var rows = new LinkedList<IReadOnlyCollection<IContent>>();
      // Prepare the headings collection
      var headings = new LinkedList<IContent>();
      // For every node grouped by its type..
      foreach (var tag in source.Nodes().OfType<XElement>().GroupBy(x => x.Name.LocalName.ToUpperInvariant()))
        // based on its type..
        switch (tag.Key)
        {
          // Is a list item
          case "ITEM":
            // Resolve the rows
            ResolveRows(ref rows, tag.Select(Linq.XtoX));
            break;
          // Is the list header
          case "LISTHEADER":
            // Resolve the headings
            ResolveHeadings(ref headings, tag.Select(Linq.XtoX));
            break;
          // Invalid list type
          default:
            continue;
        }

      Type = ResolveType(source);
      Rows = rows;
      Headings = headings;
    }

    #region Methods

    private static IListTag.ListType ResolveType(XElement node)
    {
      // Get the list type
      var type = node
        // Select the documentation node attributes
        .Attributes()
        // Select the attributes which hold the node type
        .First(attribute => attribute.Name.LocalName.Equals("type", StringComparison.InvariantCultureIgnoreCase))
        // Select the attribute type value
        .Value;

      // Identify and return the list type
      return type.ToUpperInvariant() switch
      {
        "BULLET" => IListTag.ListType.Bullet,
        "NUMBER" => IListTag.ListType.Number,
        "TABLE" => IListTag.ListType.Table,
        _ => throw new NotSupportedException()
      };
    }

    private static void ResolveHeadings(ref LinkedList<IContent> result, IEnumerable<XElement> elements)
    {
      // For every element heading..
      foreach (var element in elements)
        // add the heading
        result.AddLast(element
          // Select the documentation nodes
          .Nodes()
          // Select elements
          .OfType<XElement>()
          // Select the element node
          .Select(x => x.FirstNode)
          // Resolve the nodes to documentation elements
          .Select(ContentResolver.Resolve!)
          // Flatten the sequence
          .SelectMany(Linq.XtoX)
          // Select the heading
          .First());
    }

    private static void ResolveRows(ref LinkedList<IReadOnlyCollection<IContent>> result, IEnumerable<XElement> elements)
    {
      // For every element row..
      foreach (var element in elements)
        // add the row
        result.AddLast(element
          // Select the documentation nodes
          .Nodes()
          // Select elements
          .OfType<XElement>()
          // Select the element node
          .Select(x => x.FirstNode)
          // Resolve the nodes to documentation elements
          .Select(ContentResolver.Resolve!)
          // Flatten the sequence
          .SelectMany(Linq.XtoX)
          // Materialize to a collection
          .ToReadOnlyCollection());
    }

    #endregion
  }
}
