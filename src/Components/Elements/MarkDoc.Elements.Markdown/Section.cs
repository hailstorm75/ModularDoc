using System;
using System.Collections.Generic;
using System.Linq;
using MarkDoc.Helpers;

namespace MarkDoc.Elements.Markdown
{
  /// <summary>
  /// Class for markdown section
  /// </summary>
  public class Section
    : BaseElement, ISection
  {
    #region Properties

    /// <inheritdoc />
    public IReadOnlyCollection<IElement> Content { get; }

    /// <inheritdoc />
    public string Heading { get; }

    /// <inheritdoc />
    public int Level { get; }

    #endregion

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="content">Section content</param>
    /// <param name="heading">Element heading</param>
    /// <param name="level">Element heading level</param>
    public Section(IEnumerable<IElement> content, string heading = "", int level = 0)
    {
      Content = content.ToReadOnlyCollection();
      Heading = heading;
      Level = level;
    }

    /// <inheritdoc />
    public override IEnumerable<string> Print()
    {
      // If there is a heading..
      if (!string.IsNullOrEmpty(Heading))
      {
        // print the heading
        yield return Heading.ToHeading(Level);
        // print a line break
        yield return Environment.NewLine;
      }

      // For every element except the last..
      foreach (var element in Content.Take(Content.Count - 1))
      {
        // for every part of the element..
        foreach (var line in element.Print())
          // print it
          yield return line;

        // print a line break
        yield return Environment.NewLine;
      }

      // Assume the last printed line is an empty string
      var last = "";
      // For every part of the last element..
      foreach (var line in Content.Last().Print())
      {
        // track the printed line
        last = line;
        // print the line
        yield return line;
      }

      // If the last line was not a line break..
      if (!last.Equals(Environment.NewLine, StringComparison.InvariantCultureIgnoreCase))
        // print a line break
        yield return Environment.NewLine;
    }
  }
}
