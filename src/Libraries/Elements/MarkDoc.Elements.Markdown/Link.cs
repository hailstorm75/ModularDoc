using System;
using System.Collections.Generic;

namespace MarkDoc.Elements.Markdown
{
  public class Link
    : ILink
  {
    #region Properties

    /// <inheritdoc />
    public IText Content { get; }

    /// <inheritdoc />
    public Lazy<string> Reference { get; }

    #endregion

    public Link(IText content, Lazy<string> reference)
    {
      Content = content;
      Reference = reference;
    }

    /// <inheritdoc />
    public IEnumerable<string> Print()
    {
      if (string.IsNullOrEmpty(Reference.Value))
      {
        foreach (var line in Content.Print())
          yield return line;
      }
      else
      {
        yield return "[";
        foreach (var line in Content.Print())
          yield return line;
        yield return $"]({Reference.Value})";
      }
    }
  }
}
