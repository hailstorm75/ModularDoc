using System;
using static MarkDoc.Elements.IText;

namespace MarkDoc.Elements.Markdown
{
  public class TextElement
    : BaseElement, IText
  {
    #region Properties

    /// <inheritdoc />
    public string Content { get; }

    /// <inheritdoc />
    public TextStyle Style { get; }

    #endregion

    public TextElement(string content, TextStyle style = TextStyle.Normal)
    {
      Content = content;
      Style = style;
    }

    /// <inheritdoc />
    public override string ToString()
      => Content + Environment.NewLine;
  }
}
