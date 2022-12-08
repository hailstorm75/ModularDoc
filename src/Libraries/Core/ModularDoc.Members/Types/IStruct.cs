using System.Collections.Generic;
using ModularDoc.Members.Members;

namespace ModularDoc.Members.Types
{
  /// <summary>
  /// Interface for struct types
  /// </summary>
  public interface IStruct
    : IInterface
  {
    /// <summary>
    /// Struct constructors
    /// </summary>
    IReadOnlyCollection<IConstructor> Constructors { get; }

    /// <summary>
    /// Determines whether the struct is readonly
    /// </summary>
    bool IsReadOnly { get; }
  }
}
