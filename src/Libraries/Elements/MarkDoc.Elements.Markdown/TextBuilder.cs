using MarkDoc.Helpers;
using System.Collections.Generic;

namespace MarkDoc.Elements.Markdown
{
  public class TextBuilder
    : ITextBuilder
  {
    #region Properties

    /// <inheritdoc />
    public string Delimiter { get; }

    /// <inheritdoc />
    public IReadOnlyCollection<ITextContent> Content { get; }

    #endregion

    public TextBuilder(IEnumerable<ITextContent> content, string delimiter)
    {
      Content = content.ToReadOnlyCollection();
      Delimiter = delimiter;
    }
  }
}
