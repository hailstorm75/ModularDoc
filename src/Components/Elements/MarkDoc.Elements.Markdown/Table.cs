using System;
using System.Collections.Generic;
using System.Linq;
using MarkDoc.Helpers;

namespace MarkDoc.Elements.Markdown
{
  /// <summary>
  /// Class for markdown tables
  /// </summary>
  public class Table
    : BaseElement, ITable
  {
    #region Constants

    private const string DEL_VERTICAL = "|";
    private const string DEL_HORIZONTAL = "-";

    #endregion

    #region Properties

    /// <inheritdoc />
    public string Heading { get; }

    /// <inheritdoc />
    public int Level { get; }

    /// <inheritdoc />
    public IReadOnlyCollection<IText> Headings { get; }

    /// <inheritdoc />
    public IReadOnlyCollection<IReadOnlyCollection<IElement>> Content { get; }

    #endregion

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="headings">Table headings</param>
    /// <param name="content">Collection of rows</param>
    /// <param name="heading">Element heading</param>
    /// <param name="level">Element heading level</param>
    public Table(IEnumerable<IText> headings, IEnumerable<IReadOnlyCollection<IElement>> content, string heading = "", int level = 0)
    {
      Headings = headings.ToReadOnlyCollection();
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

      // Begin column headers with a vertical delimiter
      yield return DEL_VERTICAL;
      // For every heading..
      foreach (var heading in Headings)
      {
        // start heading with whitespace
        yield return " ";
        // for every part of a heading..
        foreach (var line in heading.Print())
          // print it
          yield return line;
        // finish heading with whitespace and a vertical line
        yield return $" {DEL_VERTICAL}";
      }

      // Print line break from headings
      yield return Environment.NewLine;

      // Being the horizontal line with a vertical delimiter
      yield return DEL_VERTICAL;
      // For the number of headings..
      for (var i = 0; i < Headings.Count; i++)
        // print parts of the horizontal line
        yield return $" {DEL_HORIZONTAL}{DEL_HORIZONTAL}{DEL_HORIZONTAL} {DEL_VERTICAL}";

      // For every row..
      foreach (var p in ProcessContentRows())
        yield return p;
    }

    private IEnumerable<string> ProcessContentRows()
    {
      foreach (var row in Content)
      {
        // break to a new line
        yield return Environment.NewLine;
        // begin building the row with a vertical delimiter
        yield return DEL_VERTICAL;

        // assume none of columns are filled by this row
        var colCount = 0;
        // for every (but not more than headings) row item..
        foreach (var item in row.Take(Headings.Count))
        {
          // increment the filled row count
          colCount++;
          // start row item with whitespace
          yield return " ";
          // for every part of a row item..
          foreach (var line in item.Print())
            // fix characters and print it
            yield return line.ReplaceNewline();
          // finish the row item with a whitespace and vertical line
          yield return $" {DEL_VERTICAL}";
        }

        // if there are more headings than there are row items..
        if (Headings.Count <= colCount)
          continue;
        for (var i = 0; i < Headings.Count - colCount; i++)
        {
          // print an empty row item
          yield return "   ";
          // finish the row item with a vertical line
          yield return DEL_VERTICAL;
        }
      }

      // print a line break
      yield return Environment.NewLine;
    }
  }
}
