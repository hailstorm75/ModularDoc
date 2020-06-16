using MarkDoc.Members.ResolvedTypes;
using System.Collections.Generic;

namespace MarkDoc.Members.Members
{
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
    IReadOnlyCollection<string> Generics { get; }
    /// <summary>
    /// Method return type
    /// </summary>
    IResType? Returns { get; }
  }
}
