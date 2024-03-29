﻿using System;
using System.Diagnostics;
using System.Xml.Linq;
using ModularDoc.Documentation.Tags;

namespace ModularDoc.Documentation.Xml.Tags
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

    internal TextTag(string content)
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
      return text.Value;
    }
  }
}
