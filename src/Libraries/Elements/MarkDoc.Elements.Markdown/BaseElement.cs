using System.Collections.Generic;

namespace MarkDoc.Elements.Markdown
{
  public abstract class BaseElement
    : IElement
  {
    public abstract IEnumerable<string> Print();
  }
}
