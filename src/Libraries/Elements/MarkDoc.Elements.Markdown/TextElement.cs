using System;
using System.Linq;
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

    /// <inheritdoc cref="IElement" />
    public override string ToString()
    {
      if (string.IsNullOrEmpty(Content))
        return string.Empty;

      return Style switch
      {
        IText.TextStyle.Normal
          => Content.CleanInvalid(),
        IText.TextStyle.CodeInline
          => $"`{Content}`",
        IText.TextStyle.Code
          => $"```csharp{Environment.NewLine}{Content}{Environment.NewLine}```",
        IText.TextStyle.Italic
          => Content.CleanInvalid().Any(x => x.Equals('*'))
            ? $"_{Content}_"
            : $"*{Content}*",
        IText.TextStyle.Bold
          => Content.CleanInvalid().Any(x => x.Equals('*'))
            ? $"__{Content}__"
            : $"**{Content}**",
        _ => throw new Exception() // TODO: Specify exception message
      };
    }
  }
}
