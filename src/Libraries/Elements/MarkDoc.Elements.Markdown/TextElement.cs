using System;

namespace MarkDoc.Elements.Markdown
{
  public class TextElement
    : BaseElement, IText
  {
    #region Properties

    /// <inheritdoc />
    public string Content { get; set; } = string.Empty;

    /// <inheritdoc />
    public IText.TextStyle Style { get; set; } = IText.TextStyle.Normal; 

    #endregion

    /// <inheritdoc />
    public override string ToString()
      => Content + Environment.NewLine;
  }
}
