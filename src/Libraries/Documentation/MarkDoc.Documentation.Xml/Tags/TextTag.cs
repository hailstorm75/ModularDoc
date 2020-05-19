using System;
using System.Diagnostics;
using System.Xml.Linq;
using MarkDoc.Documentation.Tags;

namespace MarkDoc.Documentation.Xml.Tags
{
  [DebuggerDisplay("{Content}")]
  public class TextTag
    : ITextTag
  {
    public string Content { get; }

    public TextTag(XText text)
    {
      if (text == null)
        throw new ArgumentNullException(nameof(text));

      Content = text.Value.Trim() ?? string.Empty;
    }
  }
}
