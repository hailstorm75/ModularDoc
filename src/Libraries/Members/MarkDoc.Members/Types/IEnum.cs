using System.Collections.Generic;

namespace MarkDoc.Members.Types
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
