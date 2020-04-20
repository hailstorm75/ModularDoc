using System;
using System.Collections.Generic;
using System.Text;

namespace MarkDoc.Elements.Members
{
  /// <summary>
  /// Interface for enums 
  /// </summary>
  public interface IEnum
    : IType
  {
    /// <summary>
    /// Enum fields
    /// </summary>
    IReadOnlyCollection<string> Fields { get; }
  }
}
