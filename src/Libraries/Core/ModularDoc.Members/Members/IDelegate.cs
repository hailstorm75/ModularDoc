using ModularDoc.Members.ResolvedTypes;
using System.Collections.Generic;

namespace ModularDoc.Members.Members
{
  /// <summary>
  /// Interface for delegate types
  /// </summary>
  public interface IDelegate
    : IMember
  {
    /// <summary>
    /// Delegate arguments
    /// </summary>
    IReadOnlyCollection<IArgument> Arguments { get; }
    /// <summary>
    /// Method generic arguments
    /// </summary>
    IReadOnlyDictionary<string, IReadOnlyCollection<IResType>> Generics { get; }
    /// <summary>
    /// Method return type
    /// </summary>
    IResType? Returns { get; }
  }
}
