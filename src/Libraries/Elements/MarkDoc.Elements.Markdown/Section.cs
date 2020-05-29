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

    public Section(IEnumerable<IElement>? content = default, string heading = "", int level = 0)
    {
      Content = (content ?? Enumerable.Empty<IElement>()).ToReadOnlyCollection();
      Heading = heading;
      Level = Level;
    }

    /// <inheritdoc />
    public override string ToString()
    {
      var result = new StringBuilder();
      foreach (var element in Content.Take(Content.Count - 1))
        result.AppendLine(element.ToString()).AppendLine();
      result.AppendLine(Content.Last().ToString());

      return result.ToString();
    }
  }
}
