using System;
using System.Collections.Generic;
using System.Text;

namespace MarkDoc.Elements
{
  /// <summary>
  /// Interface for properties
  /// </summary>
  public interface IProperty
  {
    /// <summary>
    /// Property name
    /// </summary>
    string Name { get; }
  }
}
