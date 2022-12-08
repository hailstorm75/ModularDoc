using System.Collections.Generic;

namespace ModularDoc.Members.ResolvedTypes
{
  /// <summary>
  /// Interface for generic resolved types
  /// </summary>
  public interface IResGeneric
    : IResType
  {
    /// <summary>
    /// Generic parameter types
    /// </summary>
    IReadOnlyCollection<IResType> Generics { get; }
  }
}
