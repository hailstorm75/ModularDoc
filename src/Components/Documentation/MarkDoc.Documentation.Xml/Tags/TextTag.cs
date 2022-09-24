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
    internal TextTag(XText text)
    {
      // If the text is null..
      if (text is null)
        // throw an exception
        throw new ArgumentNullException(nameof(text));

      // Initialize the content
      Content = text.Value.Trim();
    }

    protected TextTag(string content)
    {
      if (content is null)
        throw new ArgumentNullException(nameof(content));

      Content = content;
    }
  }

  [DebuggerDisplay("{" + nameof(Content) + "}")]
  public class CodeTag
    : TextTag
  {
    /// <inheritdoc />
    internal CodeTag(XText text)
      : base(ProcessCode(text))
    {
    }

    private static string ProcessCode(XText text)
    {
      var code = text.Value;
      var whitespaceEndIndex = code.Length - code.AsSpan().TrimStart().Length;
      var whitespace = code.Substring(1, whitespaceEndIndex - 1);
      return code.AsSpan()[1..].ToString().Replace(whitespace, string.Empty);
    }
  }
}
