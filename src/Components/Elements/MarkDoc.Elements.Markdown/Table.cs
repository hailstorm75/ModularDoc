using System;
using System.Collections.Generic;
using System.Linq;
using MarkDoc.Helpers;

namespace MarkDoc.Elements.Markdown
{
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
      var count = 0;

      if (!string.IsNullOrEmpty(Heading))
      {
        yield return Heading.ToHeading(Level);
        yield return Environment.NewLine;
      }

      // Column headers
      yield return DEL_VERTICAL;
      foreach (var heading in Headings)
      {
        count++;
        yield return " ";
        foreach (var line in heading.Print())
          yield return line;
        yield return $" {DEL_VERTICAL}";
      }

      // Horizontal line
      yield return Environment.NewLine;
      yield return DEL_VERTICAL;
      for (var i = 0; i < count; i++)
        yield return $" {DEL_HORIZONTAL}{DEL_HORIZONTAL}{DEL_HORIZONTAL} {DEL_VERTICAL}";

      foreach (var row in Content)
      {
        yield return Environment.NewLine;
        yield return DEL_VERTICAL;

        var colCount = 0;
        foreach (var item in row.Take(count))
        {
          colCount++;
          yield return " ";
          foreach (var line in item.Print())
            yield return line.ReplaceNewline();
          yield return $" {DEL_VERTICAL}";
        }

        if (count > colCount)
          for (var i = 0; i < count - colCount; i++)
          {
            yield return "   ";
            yield return DEL_VERTICAL;
          }
      }

      yield return Environment.NewLine;
    }
  }
}
