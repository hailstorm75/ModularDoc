﻿using MarkDoc.Members.Enums;
using System;

namespace MarkDoc.Members
{
  /// <summary>
  /// Interface for properties
  /// </summary>
  public interface IProperty
    : IMember
  {
    /// <summary>
    /// Property visibility
    /// </summary>
    MemberInheritance Visibility { get; }

    /// <summary>
    /// Property type
    /// </summary>
    Lazy<IType> Type { get; }

    /// <summary>
    /// Property get accessor type
    /// </summary>
    AccessorType GetAccessor { get; }

    /// <summary>
    /// Property set accessor type
    /// </summary>
    AccessorType SetAccessor { get; }
  }
}
