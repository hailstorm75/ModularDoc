using System.Collections.Generic;

namespace MarkDoc.Elements
{
  /// <summary>
  /// Interface for methods
  /// </summary>
  public interface IMethod
  {
    /// <summary>
    /// Method name
    /// </summary>
    string Name { get; }
    /// <summary>
    /// Method arguments
    /// </summary>
    IReadOnlyCollection<IArgument> Arguments { get; }
  }
}
