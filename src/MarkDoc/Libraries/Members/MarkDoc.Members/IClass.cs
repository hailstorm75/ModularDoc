using System;
using System.Collections.Generic;

namespace MarkDoc.Members
{
  /// <summary>
  /// Interface for classes
  /// </summary>
  public interface IClass
    : IInterface
  {
    /// <summary>
    /// Determines whether this class is abstract
    /// </summary>
    bool IsAbstract { get; }

    /// <summary>
    /// Inherited base class
    /// </summary>
    Lazy<IResType?> BaseClass { get; }

    /// <summary>
    /// Class constructors
    /// </summary>
    IReadOnlyCollection<IConstructor> Constructors { get; }
  }
}
