using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MarkDoc.Helpers;

namespace MarkDoc.Elements.Markdown
{
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

    public Section(IEnumerable<IElement> content, string heading = "", int level = 0)
    {
      Content = content.ToReadOnlyCollection();
      Heading = heading;
      Level = level;
    }

    /// <inheritdoc />
    public override string ToString()
    {
      var result = new StringBuilder();

      if (!string.IsNullOrEmpty(Heading))
        result.AppendLine(Heading.ToHeading(Level));

      foreach (var element in Content.Take(Content.Count - 1))
        result.AppendLine(element.ToString()).AppendLine();
      result.AppendLine(Content.Last().ToString());

      return result.ToString();
    }

    /// <inheritdoc />
    public override IEnumerable<string> Print()
    {
      if (!string.IsNullOrEmpty(Heading))
      {
        yield return Heading.ToHeading(Level);
        yield return Environment.NewLine;
      }

      foreach (var element in Content.Take(Content.Count - 1))
      {
        foreach (var line in element.Print())
          yield return line;

        yield return Environment.NewLine;
      }

      var last = "";
      foreach (var line in Content.Last().Print())
      {
        last = line;
        yield return line;
      }

      if (!last.Equals(Environment.NewLine, StringComparison.InvariantCultureIgnoreCase))
        yield return Environment.NewLine;
    }
  }
}
