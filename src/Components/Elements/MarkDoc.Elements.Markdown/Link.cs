using System;
using System.Collections.Generic;

namespace MarkDoc.Elements.Markdown
{
  /// <summary>
  /// Class for representing markdown links
  /// </summary>
  public class Link
    : ILink
  {
    #region Properties

    /// <inheritdoc />
    public IText Content { get; }

    /// <inheritdoc />
    public Lazy<string> Reference { get; }

    #endregion

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="content">Link content</param>
    /// <param name="reference">Link target reference</param>
    public Link(IText content, Lazy<string> reference)
    {
      Content = content;
      Reference = reference;
    }

    /// <inheritdoc />
    public IEnumerable<string> Print()
    {
      // If the reference does not exist..
      if (string.IsNullOrEmpty(Reference.Value))
      {
        // for every element of the content..
        foreach (var line in Content.Print())
          // print it out
          yield return line;
      }
      // Otherwise..
      else
      {
        // print the begging of the link
        yield return "[";
        // for every element of the content..
        foreach (var line in Content.Print())
          // print it out
          yield return line;
        // close up the link
        yield return $"]({Reference.Value})";
      }
    }
  }
}
