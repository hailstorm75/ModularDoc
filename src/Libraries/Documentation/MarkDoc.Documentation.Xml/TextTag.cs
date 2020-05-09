using MarkDoc.Documentation.Tags;
using System;
using System.Diagnostics;
using System.Xml.Linq;

namespace MarkDoc.Documentation.Xml
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
