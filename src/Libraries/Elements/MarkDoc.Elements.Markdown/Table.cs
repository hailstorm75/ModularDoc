using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

    public Table(IEnumerable<IText>? headings = default, IEnumerable<IReadOnlyCollection<IElement>>? content = default, string heading = "", int level = 0)
    {
      Headings = (headings ?? Enumerable.Empty<IText>()).ToReadOnlyCollection();
      Content = (content ?? Enumerable.Empty<IReadOnlyCollection<IElement>>()).ToReadOnlyCollection();
      Heading = heading;
      Level = level;
    }

    /// <inheritdoc />
    public override string ToString()
    {
      var result = new StringBuilder();
      var count = 0;

      // Column headers
      result.Append(DEL_VERTICAL);
      foreach (var heading in Headings)
      {
        count++;
        result.Append(" ").Append(heading).Append(" ").Append(DEL_VERTICAL);
      }

      // Horizontal line
      result.Append("\n").Append(DEL_VERTICAL);
      for (var i = 0; i < count; i++)
        result.Append(" ")
          .Append(DEL_HORIZONTAL)
          .Append(DEL_HORIZONTAL)
          .Append(DEL_HORIZONTAL)
          .Append(" ")
          .Append(DEL_VERTICAL);

      foreach (var row in Content)
      {
        result.Append(Environment.NewLine).Append(DEL_VERTICAL);

        var colCount = 0;
        foreach (var item in row.Take(count))
        {
          colCount++;
          result.Append(" ").Append(item).Append(" ").Append(DEL_VERTICAL);
        }

        if (count > colCount)
          for (int i = 0; i < count - colCount; i++)
            result.Append("   ").Append(DEL_VERTICAL);
      }

      return result.ToString();
    }
  }
}
