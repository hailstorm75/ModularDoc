using dnlib.DotNet;
using MarkDoc.Helpers;
using MarkDoc.Members.Dnlib.Properties;
using MarkDoc.Members.Enums;
using System;
using System.Collections.Generic;
using System.Threading;
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
    public IReadOnlyCollection<Lazy<IResType>> InheritedInterfaces { get; }

    /// <inheritdoc />
    public IReadOnlyDictionary<string, (Variance variance, IReadOnlyCollection<Lazy<IResType>> constraints)> Generics { get; }

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

      InheritedInterfaces = ResolveInterfaces(source).ToArray();
      NestedTypes = source.NestedTypes.Where(x => !x.Name.String.StartsWith('<'))
                                      .Select(x => Resolver.Instance.ResolveType(x, source))
                                      .ToArray();
      Generics = source.GenericParameters.Except(parent?.GenericParameters ?? Enumerable.Empty<GenericParam>(), EqualityComparerEx<GenericParam>.Create(x => x.Name, x => x.Name))
                                         .ToDictionary(x => x.Name.String, ResolveParameter);

      Methods = source.Methods.Where(x => !x.SemanticsAttributes.HasFlag(MethodSemanticsAttributes.Getter)
                                       && !x.SemanticsAttributes.HasFlag(MethodSemanticsAttributes.Setter)
                                       && !x.IsPrivate
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

    private static IEnumerable<Lazy<IResType>> ResolveInterfaces(dnlib.DotNet.TypeDef source)
      => source.Interfaces.Select(x => new Lazy<IResType>(() => Resolver.Instance.Resolve(x.Interface.ToTypeSig()), LazyThreadSafetyMode.ExecutionAndPublication));

    private static (Variance, IReadOnlyCollection<Lazy<IResType>>) ResolveParameter(GenericParam parameter)
    {
      var variance = ResolveVariance(parameter.Variance);

      if (!parameter.HasGenericParamConstraints)
        return (variance, Enumerable.Empty<Lazy<IResType>>().ToArray());

      return (variance, parameter.GenericParamConstraints.Select(x => new Lazy<IResType>(() => ResolveType(x), LazyThreadSafetyMode.ExecutionAndPublication)).ToArray());
    }

    private static IResType ResolveType(GenericParamConstraint x)
      => Resolver.Instance.Resolve(x.Constraint.ToTypeSig());

    private static Variance ResolveVariance(GenericParamAttributes attributes)
      => attributes switch
      {
        GenericParamAttributes.NonVariant => Variance.NonVariant,
        GenericParamAttributes.Covariant => Variance.Covariant,
        GenericParamAttributes.Contravariant => Variance.Contravariant,
        _ => throw new NotSupportedException(Resources.varianceInvalid),
      };

    #endregion
  }
}
