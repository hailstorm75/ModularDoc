using dnlib.DotNet;
using MarkDoc.Helpers;
using MarkDoc.Members.Dnlib.Properties;
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
    public IReadOnlyCollection<IResType> InheritedInterfaces { get; }

    /// <inheritdoc />
    public IReadOnlyDictionary<string, (Variance variance, IReadOnlyCollection<IResType> constraints)> Generics { get; }

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
    internal InterfaceDef(IResolver resolver, dnlib.DotNet.TypeDef source, dnlib.DotNet.TypeDef? parent)
      : base(resolver, source, parent)
    {
      if (source == null)
        throw new ArgumentNullException(nameof(source));

      var generics = source.ResolveTypeGenerics();
      InheritedInterfaces = ResolveInterfaces(source, generics).ToReadOnlyCollection();
      NestedTypes = source.NestedTypes.Where(x => !x.Name.String.StartsWith('<'))
                                      .Select(x => Resolver.ResolveType(x, source))
                                      .ToReadOnlyCollection();
      Generics = source.GenericParameters.Except(parent?.GenericParameters ?? Enumerable.Empty<GenericParam>(), EqualityComparerEx<GenericParam>.Create(x => x.Name, x => x.Name))
                                         .ToDictionary(x => x.Name.String, x => ResolveParameter(x, generics));

      Methods = source.Methods.Where(x => !x.SemanticsAttributes.HasFlag(MethodSemanticsAttributes.Getter)
                                       && !x.SemanticsAttributes.HasFlag(MethodSemanticsAttributes.Setter)
                                       && !x.IsPrivate
                                       && !x.IsConstructor)
                              .Select(x => new MethodDef(resolver, x))
                              .ToReadOnlyCollection();
      Properties = source.Properties.Select(x => PropertyDef.Initialize(resolver, x))
                                    .WhereNotNull()
                                    .ToReadOnlyCollection();
      Events = source.Events.Select(x => new EventDef(resolver, x))
                            .ToReadOnlyCollection();
    }

    #region Methods

    private IEnumerable<IResType> ResolveInterfaces(dnlib.DotNet.TypeDef source, IReadOnlyDictionary<string, string> outerArgs)
      => source.Interfaces.Select(x => Resolver.Resolve(x.Interface.ToTypeSig(), outerArgs));

    private (Variance, IReadOnlyCollection<IResType>) ResolveParameter(GenericParam parameter, IReadOnlyDictionary<string, string> generics)
    {
      var variance = ResolveVariance(parameter.Variance);

      if (!parameter.HasGenericParamConstraints)
        return (variance, Enumerable.Empty<IResType>().ToArray());

      return (variance, parameter.GenericParamConstraints.Select(x => ResolveType(x, generics)).ToReadOnlyCollection());
    }

    private IResType ResolveType(GenericParamConstraint x, IReadOnlyDictionary<string, string> generics)
      => Resolver.Resolve(x.Constraint.ToTypeSig(), generics) ?? throw new Exception();

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
