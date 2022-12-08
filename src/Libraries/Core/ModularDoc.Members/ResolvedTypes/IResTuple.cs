using System.Collections.Generic;

namespace ModularDoc.Members.ResolvedTypes
{
  /// <summary>
  /// Interface for resolved tuples
  /// </summary>
  public interface IResTuple
    : IResType
  {
    /// <summary>
    /// Tuple fields
    /// </summary>
    IReadOnlyCollection<(string name, IResType type)> Fields { get; }
    /// <summary>
    /// Determines whether the tuple is a value tuple
    /// </summary>
    bool IsValueTuple { get; }
  }
}
