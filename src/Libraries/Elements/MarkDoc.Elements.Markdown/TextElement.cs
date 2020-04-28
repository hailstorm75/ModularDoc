using System;

namespace MarkDoc.Elements.Markdown
{
  public class TextElement
    : BaseElement, IText
  {
    /// <inheritdoc />
    public string Content { get; set; } = string.Empty;

    /// <inheritdoc />
    public override string ToString()
      => Content + Environment.NewLine;
  }
}
