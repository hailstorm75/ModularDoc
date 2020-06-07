using MarkDoc.Helpers;
using System.Collections.Generic;
using System.Linq;

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

    public override string ToString()
      => string.Join(Delimiter, Content.Select(x => x.ToString()).Where(x => !string.IsNullOrEmpty(x)));
  }
}
