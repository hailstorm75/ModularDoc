using System.Collections.Generic;

namespace ModularDoc.Members.Members
{
  /// <summary>
  /// Interface for type constructors
  /// </summary>
  public interface IConstructor
    : IMember
  {
    /// <summary>
    /// Method arguments
    /// </summary>
    IReadOnlyCollection<IArgument> Arguments { get; }
  }
}
