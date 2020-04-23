using dnlib.DotNet;
using MarkDoc.Members.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace MarkDoc.Members.Dnlib
{
  [DebuggerDisplay(nameof(InterfaceDef) + ": {Name}")]
  public class InterfaceDef
    : TypeDef, IInterface
  {
    #region Properties

    /// <inheritdoc />
    public IReadOnlyCollection<Lazy<IInterface>> InheritedInterfaces { get; }

    /// <inheritdoc />
    public IReadOnlyDictionary<string, (Variance variance, IReadOnlyCollection<Lazy<IType>> constraints)> Generics { get; }

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
    public InterfaceDef(dnlib.DotNet.TypeDef source)
      : base(source)
    {
      if (source == null)
        throw new ArgumentNullException(nameof(source));

      var interfaces = source.NestedTypes;
      Generics = source.GenericParameters.ToDictionary(x => x.Name.String, ResolveParameter);

      Methods = source.Methods.Where(x => !x.SemanticsAttributes.HasFlag(MethodSemanticsAttributes.Getter)
                                       && !x.SemanticsAttributes.HasFlag(MethodSemanticsAttributes.Setter)
                                       && !x.IsConstructor)
                              .Select(x => new MethodDef(x))
                              .ToArray();
      Properties = source.Properties.Select(x => new PropertyDef(x))
                                    .ToArray();
      var events = source.Events;
    }

    private static (Variance, IReadOnlyCollection<Lazy<IType>>) ResolveParameter(GenericParam parameter)
    {
      var variance = ResolveVariance(parameter.Variance);

      if (!parameter.HasGenericParamConstraints)
        return (variance, Enumerable.Empty<Lazy<IType>>().ToArray());

      // TODO: Implement lazy resolver
      return (variance, parameter.GenericParamConstraints.Select(x => new Lazy<IType>(() => default)).ToArray());
    }

    private static Variance ResolveVariance(GenericParamAttributes attributes)
    {
      switch (attributes)
      {
        case GenericParamAttributes.NonVariant:
          return Variance.NonVariant;
        case GenericParamAttributes.Covariant:
          return Variance.Covariant;
        case GenericParamAttributes.Contravariant:
          return Variance.Contravariant;
        default:
          throw new NotSupportedException("Invalid variance attribute value");
      }
    }
  }
}
