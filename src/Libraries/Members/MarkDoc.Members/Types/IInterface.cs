using System;
using System.Collections.Generic;
using MarkDoc.Members.Enums;
using MarkDoc.Members.Members;
using MarkDoc.Members.ResolvedTypes;

namespace MarkDoc.Members.Types
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
    IReadOnlyCollection<IResType> InheritedInterfaces { get; }
    /// <summary>
    /// Generics name, and their variance and constraints
    /// </summary>
    IReadOnlyDictionary<string, (Variance variance, IReadOnlyCollection<IResType> constraints)> Generics { get; }
    /// <summary>
    /// Collection of delegates
    /// </summary>
    IReadOnlyCollection<IDelegate> Delegates { get; }
    /// <summary>
    /// Collection of nested types
    /// </summary>
    IReadOnlyCollection<IType> NestedTypes { get; }
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
    /// <summary>
    /// Members inherited from derived types
    /// </summary>
    Lazy<IReadOnlyDictionary<IMember, IInterface>> InheritedTypes { get; }
  }
}
