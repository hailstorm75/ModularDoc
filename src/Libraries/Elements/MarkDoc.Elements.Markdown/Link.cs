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

    public override string ToString()
      => $"[{Content.ToString()}]({Reference.Value})";

    /// <inheritdoc />
    public IEnumerable<string> Print()
    {
      yield return $"[{Content.ToString()}]({Reference.Value})";
    }
  }
}
