using System.Collections.Generic;
using MarkDoc.Members.Members;
using MarkDoc.Members.ResolvedTypes;

namespace MarkDoc.Members.Types
{
  /// <summary>
  /// Interface for classes
  /// </summary>
  public interface IClass
    : IInterface
  {
    /// <summary>
    /// Determines whether this class is static
    /// </summary>
    bool IsStatic { get; }

    /// <summary>
    /// Determines whether this class is sealed
    /// </summary>
    bool IsSealed { get; }

    /// <summary>
    /// Determines whether this class is abstract
    /// </summary>
    bool IsAbstract { get; }

    /// <summary>
    /// Inherited base class
    /// </summary>
    IResType? BaseClass { get; }

    /// <summary>
    /// Class constructors
    /// </summary>
    IReadOnlyCollection<IConstructor> Constructors { get; }
  }
}
