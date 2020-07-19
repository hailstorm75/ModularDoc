using System.Collections.Generic;

namespace MarkDoc.Elements.Markdown
{
  public abstract class BaseElement
    : IElement
  {
    /// <inheritdoc />
    public new abstract string ToString();

    public abstract IEnumerable<string> Print();
  }
}
