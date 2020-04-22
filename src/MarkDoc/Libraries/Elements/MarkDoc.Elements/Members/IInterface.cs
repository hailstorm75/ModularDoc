using MarkDoc.Elements.Members.Enums;
using System;
using System.Collections.Generic;

namespace MarkDoc.Elements.Members
{
  /// <summary>
  /// Interface for interface types
  /// </summary>
  public interface IInterface
    : IType
  {
    /// <summary>
    /// Collection of inherited interfaces
    /// </summary>
    IReadOnlyCollection<Lazy<IInterface>> InheritedInterfaces { get; }
    /// <summary>
    /// Generics name, and their variance and constraints
    /// </summary>
    IReadOnlyDictionary<string, (Variance variance, IReadOnlyCollection<Lazy<IInterface>> constraints)> Generics { get; }

    /// <summary>
    /// Collection of nested enums
    /// </summary>
    IReadOnlyCollection<Lazy<IInterface>> NestedEnums { get; }
    /// <summary>
    /// Collection of events
    /// </summary>
    IReadOnlyCollection<IEvent> Events { get; }
    /// <summary>
    /// Collection of methods
    /// </summary>
    IReadOnlyCollection<IMethod> Methods { get; }
    /// <summary>
    /// Collection of properties
    /// </summary>
    IReadOnlyCollection<IProperty> Properties { get; }
  }
}
