using System.Collections.Generic;

namespace MarkDoc.Elements
{
  /// <summary>
  /// Interface for elements
  /// </summary>
  public interface IElement
  {
    IEnumerable<string> Print();
  }
}
