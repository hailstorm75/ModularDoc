using MarkDoc.Members.Enums;
using System;

namespace MarkDoc.Members
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
    /// Determines whether the method is asynchronous
    /// </summary>
    bool IsAsync { get; }

    /// <summary>
    /// Method return type
    /// </summary>
    Lazy<IType?> Returns { get; }
  }
}
