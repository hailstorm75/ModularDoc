using System;
using System.Collections.Generic;
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
    public override IEnumerable<string> Print()
    {
      if (string.IsNullOrEmpty(Content))
        yield return string.Empty;
      else
        yield return Style switch
        {
          TextStyle.Normal
            => Content.CleanInvalid(),
          TextStyle.CodeInline
            => $"`{Content}`",
          TextStyle.Code
            => $"```csharp{Environment.NewLine}{Content}{Environment.NewLine}```",
          TextStyle.Italic
            => Content.CleanInvalid().Any(x => x.Equals('*'))
              ? $"_{Content}_"
              : $"*{Content}*",
          TextStyle.Bold
            => Content.CleanInvalid().Any(x => x.Equals('*'))
              ? $"__{Content}__"
              : $"**{Content}**",
          _ => throw new Exception() // TODO: Specify exception message
        };
    }
  }
}
