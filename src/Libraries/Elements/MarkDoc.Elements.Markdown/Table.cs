using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    public string Heading { get; set; } = string.Empty;

    /// <inheritdoc />
    public int Level { get; set; } = 0;

    /// <inheritdoc />
    public IReadOnlyCollection<IText> Headings { get; set; } = Enumerable.Empty<IText>().ToArray();

    /// <inheritdoc />
    public IReadOnlyCollection<IReadOnlyCollection<IElement>> Content { get; set; } = Enumerable.Empty<IReadOnlyCollection<IElement>>().ToArray();

    #endregion

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
