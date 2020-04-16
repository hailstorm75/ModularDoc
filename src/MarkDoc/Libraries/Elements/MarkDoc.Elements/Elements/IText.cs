using System;
using System.Collections.Generic;
using System.Text;

namespace MarkDoc.Elements.Elements
{
  /// <summary>
  /// Interface for teext elements
  /// </summary>
  public interface IText
  {
    /// <summary>
    /// Text content
    /// </summary>
    string Content { get; }
  }
}
