using System.Collections.Generic;
using ModularDoc.Members.Members;

namespace ModularDoc.Members.Types
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
