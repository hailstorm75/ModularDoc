using System.Collections.Generic;
using MarkDoc.Members.ResolvedTypes;

namespace MarkDoc.Members
{
  /// <summary>
  /// Interface for interface types
  /// </summary>
  public interface IInterface
    : IStruct
  {
    /// <summary>
    /// Collection of inherited interfaces
    /// </summary>
    IReadOnlyCollection<IResType> InheritedInterfaces { get; }
  }
}
