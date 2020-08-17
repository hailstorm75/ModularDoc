using System;
using System.Diagnostics;
using System.Xml.Linq;
using MarkDoc.Documentation.Tags;

namespace MarkDoc.Documentation.Xml.Tags
{
  /// <summary>
  /// Documentation tag containing text
  /// </summary>
  [DebuggerDisplay("{" + nameof(Content) + "}")]
  public class TextTag
    : ITextTag
  {
    /// <inheritdoc />
    public string Content { get; }

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="text">Content source</param>
    public TextTag(XText text)
    {
      // If the text is null..
      if (text is null)
        // throw an exception
        throw new ArgumentNullException(nameof(text));

      // Initialize the content
      Content = text.Value.Trim() ?? string.Empty;
    }
  }
}
