﻿using MarkDoc.Elements.Members.Enums;
using System;

namespace MarkDoc.Elements.Members
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
    MemberVisibility Visibility { get; }

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
