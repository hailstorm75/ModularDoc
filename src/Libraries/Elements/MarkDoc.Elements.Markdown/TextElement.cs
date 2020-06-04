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

    /// <inheritdoc />
    public override string ToString()
      => Style switch
      {
        IText.TextStyle.Normal
          => Content,
        IText.TextStyle.CodeInline
          => $"`{Content}`",
        IText.TextStyle.Code
          => $"```csharp{Environment.NewLine}{Content}{Environment.NewLine}```",
        IText.TextStyle.Italic
          => Content.Any(x => x.Equals('*'))
            ? $"_{Content}_"
            : $"*{Content}*",
        IText.TextStyle.Bold
          => Content.Any(x => x.Equals('*'))
            ? $"__{Content}__"
            : $"**{Content}**",
        _ => throw new Exception() // TODO: Specify exception message
      };
  }
}
