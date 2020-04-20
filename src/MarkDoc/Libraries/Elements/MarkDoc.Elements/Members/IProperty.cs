using MarkDoc.Elements.Members;
using MarkDoc.Elements.Members.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarkDoc.Elements.Members
{
  /// <summary>
  /// Interface for properties
  /// </summary>
  public interface IProperty
    : IMember
  {
    /// <summary>
    /// Is property static
    /// </summary>
    bool IsStatic { get; }

    /// <summary>
    /// Property type
    /// </summary>
    Lazy<IType> Type { get; }

    AccessorType GetAccessor { get; }
    AccessorType SetAccessor { get; }
  }
}
