using System;
using System.Collections.Generic;
using MarkDoc.Helpers;
using static MarkDoc.Elements.IList;

namespace MarkDoc.Elements.Markdown
{
  /// <summary>
  /// Class for representing markdown lists
  /// </summary>
  public class ListElement
    : BaseElement, IList
  {
    #region Properties

    /// <inheritdoc />
    public ListType Type { get; }

    /// <inheritdoc />
    public string Heading { get; }

    /// <inheritdoc />
    public int Level { get; }

    /// <inheritdoc />
    public IReadOnlyCollection<IElement> Content { get; }

    #endregion

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="content">List content</param>
    /// <param name="type">List type</param>
    /// <param name="heading">Element heading</param>
    /// <param name="level">Element heading level</param>
    public ListElement(IEnumerable<IElement> content, ListType type = ListType.Dotted, string heading = "", int level = 0)
    {
      Content = content.ToReadOnlyCollection();
      Type = type;
      Heading = heading;
      Level = level;
    }

    /// <inheritdoc />
    public override IEnumerable<string> Print()
      => Print(1);

    /// <summary>
    /// Converts given <see cref="ListElement"/> to a sequence of strings
    /// </summary>
    /// <param name="indent">List indentation</param>
    /// <returns>Strings to export</returns>
    public IEnumerable<string> Print(int indent)
    {
      // If there is a heading..
      if (!string.IsNullOrEmpty(Heading))
      {
        // print it
        yield return Heading.ToHeading(Level);
        // print a line break
        yield return Environment.NewLine;
      }

      // For every element in the content
      foreach (var p in ProcessListElements(indent))
        yield return p;
    }

    private IEnumerable<string> ProcessListElements(int indent)
    {
      // Initialize the start index for the first list element
      var index = 0;

      foreach (var item in Content)
      {
        // If the element is a list..
        if (item is IList list)
        {
          // For event element in the nested list..
          foreach (var line in list.Print(indent + 1))
            // print it
            yield return line;

          // Move to the next element
          continue;
        }

        // Print out the indentation
        yield return $"{new string(' ', indent * 2 - 1)}";
        if (Type == ListType.Numbered)
          yield return $"{++index}. ";
        else if (Type == ListType.Dotted)
          yield return "- ";

        // For every part of the element..
        foreach (var line in item.Print())
        {
          // print it
          yield return line;
          // print a line break
          yield return Environment.NewLine;
        }
      }
    }
  }
}
