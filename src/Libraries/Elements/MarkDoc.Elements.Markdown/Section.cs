using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarkDoc.Elements.Markdown
{
  public class Section
    : BaseElement, ISection
  {
    #region Properties

    /// <inheritdoc />
    public IReadOnlyCollection<IElement> Content { get; set; } = Array.Empty<IElement>();

    /// <inheritdoc />
    public string Heading { get; set; } = string.Empty;

    /// <inheritdoc />
    public int Level { get; set; } = 0;

    #endregion

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
