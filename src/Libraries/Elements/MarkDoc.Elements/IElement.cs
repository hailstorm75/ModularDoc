using System;
using System.Collections.Generic;

namespace MarkDoc.Elements
{
  /// <summary>
  /// Interface for elements
  /// </summary>
  public interface IElement
  {
    /// <summary>
    /// Converts given element to a string
    /// </summary>
    /// <returns>Conversion result</returns>
    [Obsolete("This method is obsolete. Consider using " + nameof(Print) + " instead.")]
    string ToString();

    IEnumerable<string> Print();
  }
}
