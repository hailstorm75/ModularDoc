using System;
using System.Collections.Generic;
using System.Linq;
using static MarkDoc.Elements.IText;

namespace MarkDoc.Elements.Markdown
{
  /// <summary>
  /// Class for markdown text
  /// </summary>
  public class TextElement
    : BaseElement, IText
  {
    #region Properties

    /// <inheritdoc />
    public string Content { get; }

    /// <inheritdoc />
    public TextStyle Style { get; }

    #endregion

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="content">Text content</param>
    /// <param name="style">Text style</param>
    public TextElement(string content, TextStyle style = TextStyle.Normal)
    {
      Content = content;
      Style = style;
    }

    /// <inheritdoc />
    public override IEnumerable<string> Print()
    {
      // If there is not content..
      if (string.IsNullOrEmpty(Content))
        // print an empty
        yield return string.Empty;
      // Otherwise..
      else
        // depending on the text style..
        yield return Style switch
        {
          // output an non-stylized text
          TextStyle.Normal
            => Content.CleanInvalid(),
          // output inline code
          TextStyle.CodeInline
            => $"`{Content}`",
          // output multiline code
          TextStyle.Code
            => $"```csharp{Environment.NewLine}{Content}{Environment.NewLine}```",
          // output italic text
          TextStyle.Italic
            => Content.CleanInvalid().Any(x => x.Equals('*'))
              ? $"_{Content}_"
              : $"*{Content}*",
          // output bold text
          TextStyle.Bold
            => Content.CleanInvalid().Any(x => x.Equals('*'))
              ? $"__{Content}__"
              : $"**{Content}**",
          // the style is unknown
          _ => throw new NotSupportedException()
        };
    }
  }
}
