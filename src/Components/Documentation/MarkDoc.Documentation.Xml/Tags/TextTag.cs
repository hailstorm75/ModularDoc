using System;
using System.Diagnostics;
using System.Xml.Linq;
using MarkDoc.Documentation.Tags;

namespace MarkDoc.Documentation.Xml.Tags
{
  [DebuggerDisplay("{" + nameof(Content) + "}")]
  public class TextTag
    : ITextTag
  {
    /// <inheritdoc />
    public string Content { get; }

    public TextTag(XText text)
    {
      if (text is null)
        throw new ArgumentNullException(nameof(text));

      Content = text.Value.Trim() ?? string.Empty;
    }
  }
}
