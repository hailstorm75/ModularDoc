using System.Collections.Generic;
using MarkDoc.Members.Members;

namespace MarkDoc.Members.Types
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
  }
}
