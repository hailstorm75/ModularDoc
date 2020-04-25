using MarkDoc.Members.Enums;
using System;
using System.Collections.Generic;

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
    /// Method generic arguments
    /// </summary>
    IReadOnlyCollection<string> Generics { get; }

    /// <summary>
    /// Determines whether the method is asynchronous
    /// </summary>
    bool IsAsync { get; }

    /// <summary>
    /// Method return type
    /// </summary>
    Lazy<IResType?> Returns { get; }
  }
}
