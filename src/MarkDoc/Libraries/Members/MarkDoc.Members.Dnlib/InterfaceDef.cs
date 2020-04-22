using MarkDoc.Members.Enums;
using System;
using System.Collections.Generic;

namespace MarkDoc.Members.Dnlib
{
  public class InterfaceDef
    : TypeDef, IInterface
  {
    #region Properties

    /// <inheritdoc />
    public IReadOnlyCollection<Lazy<IInterface>> InheritedInterfaces { get; }

    /// <inheritdoc />
    public IReadOnlyDictionary<string, (Variance variance, IReadOnlyCollection<Lazy<IInterface>> constraints)> Generics { get; }

    /// <inheritdoc />
    public IReadOnlyCollection<Lazy<IInterface>> NestedEnums { get; }

    /// <inheritdoc />
    public IReadOnlyCollection<IEvent> Events { get; }

    /// <inheritdoc />
    public IReadOnlyCollection<IMethod> Methods { get; }

    /// <inheritdoc />
    public IReadOnlyCollection<IProperty> Properties { get; }

    #endregion

    /// <summary>
    /// Default constructor
    /// </summary>
    public InterfaceDef()
      : base()
    {

    }
  }
}
