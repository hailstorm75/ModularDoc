using dnlib.DotNet;
using MarkDoc.Helpers;
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
    public IReadOnlyCollection<IType> NestedTypes { get; }

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
    public InterfaceDef(dnlib.DotNet.TypeDef source, dnlib.DotNet.TypeDef? parent)
      : base(source, parent)
    {
      if (source == null)
        throw new ArgumentNullException(nameof(source));

      // TODO: Implement type resolver
      InheritedInterfaces = Array.Empty<Lazy<IInterface>>();
      NestedTypes = source.NestedTypes.Where(x => !x.Name.String.StartsWith('<'))
                                      .Select(x => Resolver.Instance.Resolve(x, source))
                                      .ToArray();
      Generics = source.GenericParameters.Except(parent?.GenericParameters ?? Enumerable.Empty<GenericParam>(), EqualityComparerEx<GenericParam>.Create(x => x.Name, x => x.Name))
                                         .ToDictionary(x => x.Name.String, ResolveParameter);

      Methods = source.Methods.Where(x => !x.SemanticsAttributes.HasFlag(MethodSemanticsAttributes.Getter)
                                       && !x.SemanticsAttributes.HasFlag(MethodSemanticsAttributes.Setter)
                                       && !x.IsConstructor)
                              .Select(x => new MethodDef(x))
                              .ToArray();
      Properties = source.Properties.Select(x => PropertyDef.Initialize(x))
                                    .Where(x => x != null)
                                    .Cast<PropertyDef>()
                                    .ToArray();
      Events = source.Events.Select(x => new EventDef(x))
                            .ToArray();
    }

    #region Methods

    private static (Variance, IReadOnlyCollection<Lazy<IType>>) ResolveParameter(GenericParam parameter)
    {
      var variance = ResolveVariance(parameter.Variance);

      if (!parameter.HasGenericParamConstraints)
        return (variance, Enumerable.Empty<Lazy<IType>>().ToArray());

      // TODO: Implement lazy resolver
      return (variance, parameter.GenericParamConstraints.Select(x => new Lazy<IType>(() => default)).ToArray());
    }

    private static Variance ResolveVariance(GenericParamAttributes attributes)
      => attributes switch
      {
        GenericParamAttributes.NonVariant => Variance.NonVariant,
        GenericParamAttributes.Covariant => Variance.Covariant,
        GenericParamAttributes.Contravariant => Variance.Contravariant,
        _ => throw new NotSupportedException("Invalid variance attribute value"),
      };

    #endregion
  }
}
