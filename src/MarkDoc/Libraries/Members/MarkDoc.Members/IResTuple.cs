using System.Collections.Generic;

namespace MarkDoc.Members
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
    IReadOnlyCollection<(string, IResType)> Fields { get; }
    /// <summary>
    /// Determines whether the tuple is a value tuple
    /// </summary>
    bool IsValueTuple { get; }
  }
}
