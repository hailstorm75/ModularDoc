using System.Collections.Generic;
using MarkDoc.Members.Enums;
using MarkDoc.Members.ResolvedTypes;

namespace MarkDoc.Members.Members
{
  /// <summary>
  /// Interface for methods
  /// </summary>
  public interface IMethod
    : IConstructor
  {
    /// <summary>
    /// Method visibility
    /// </summary>
    MemberInheritance Inheritance { get; }

    /// <summary>
    /// Method generic arguments
    /// </summary>
    IReadOnlyCollection<string> Generics { get; }

    /// <summary>
    /// Determines whether the method is asynchronous
    /// </summary>
    bool IsAsync { get; }

    /// <summary>
    /// Operator type
    /// </summary>
    OperatorType Operator { get; }

    /// <summary>
    /// Method return type
    /// </summary>
    IResType? Returns { get; }
  }
}
