using System.Collections.Generic;

namespace MarkDoc.Elements.Markdown
{
  /// <summary>
  /// Base class for elements
  /// </summary>
  public abstract class BaseElement
    : IElement
  {
    /// <inheritdoc />
    public abstract IEnumerable<string> Print();
  }
}
