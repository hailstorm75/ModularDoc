using System.Collections.Generic;
using MarkDoc.Members.Members;

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
    IReadOnlyCollection<IEnumField> Fields { get; }
  }
}
