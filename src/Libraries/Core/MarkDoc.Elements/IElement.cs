using System.Collections.Generic;

namespace MarkDoc.Elements
{
  /// <summary>
  /// Interface for elements
  /// </summary>
  public interface IElement
  {
    /// <summary>
    /// Converts given element to a sequence of strings
    /// </summary>
    /// <returns>Strings to export</returns>
    IEnumerable<string> Print();
  }
}
