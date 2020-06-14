using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using dnlib.DotNet;
using MarkDoc.Helpers;
using MarkDoc.Members.Dnlib.Properties;
using MarkDoc.Members.Enums;
using MarkDoc.Members.ResolvedTypes;
using MarkDoc.Members.Types;

namespace MarkDoc.Members.Dnlib.Types
{
  [DebuggerDisplay(nameof(InterfaceDef) + (": {" + nameof(Name) + "}"))]
  public class InterfaceDef
    : StructDef, IInterface
  {
    #region Properties

    /// <inheritdoc />
    public IReadOnlyCollection<IResType> InheritedInterfaces { get; }

    #endregion

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="resolver">Type resolver instance</param>
    /// <param name="source">Type source</param>
    /// <param name="parent">Nested type parent</param>
    internal InterfaceDef(IResolver resolver, dnlib.DotNet.TypeDef source, dnlib.DotNet.TypeDef? parent)
      : base(resolver, source, parent, ResolveGenerics(resolver, source, parent, out var generics))
    {
      if (source is null)
        throw new ArgumentNullException(nameof(source));

      InheritedInterfaces = ResolveInterfaces(source, generics).ToReadOnlyCollection();
    }

    #region Methods

    private IEnumerable<IResType> ResolveInterfaces(dnlib.DotNet.TypeDef source, IReadOnlyDictionary<string, string> outerArgs)
      => source.Interfaces.Select(x => Resolver.Resolve(x.Interface.ToTypeSig(), outerArgs));

    private static IReadOnlyDictionary<string, (Variance variance, IReadOnlyCollection<IResType>)> ResolveGenerics(IResolver resolver, dnlib.DotNet.TypeDef source, dnlib.DotNet.TypeDef? parent, out IReadOnlyDictionary<string, string> generics)
    {
      IResType ResolveType(GenericParamConstraint x, IReadOnlyDictionary<string, string> generics)
        => resolver.Resolve(x.Constraint.ToTypeSig(), generics) ?? throw new Exception();

      (Variance, IReadOnlyCollection<IResType>) ResolveParameter(GenericParam parameter, IReadOnlyDictionary<string, string> generics)
      {
        var variance = ResolveVariance(parameter.Variance);

        return !parameter.HasGenericParamConstraints
          ? (variance, Enumerable.Empty<IResType>().ToArray())
          : (variance, parameter.GenericParamConstraints.Select(x => ResolveType(x, generics)).ToReadOnlyCollection());
      }

      static Variance ResolveVariance(GenericParamAttributes attributes)
        => attributes switch
        {
          GenericParamAttributes.NonVariant => Variance.NonVariant,
          GenericParamAttributes.Covariant => Variance.Covariant,
          GenericParamAttributes.Contravariant => Variance.Contravariant,
          _ => throw new NotSupportedException(Resources.varianceInvalid),
        };

      var result = source.ResolveTypeGenerics();
      generics = result;

      return source.GenericParameters.Except(parent?.GenericParameters ?? Enumerable.Empty<GenericParam>(), EqualityComparerEx<GenericParam>.Create(x => x.Name, x => x.Name))
                                     .ToDictionary(x => x.Name.String, x => ResolveParameter(x, result));
    }

    #endregion
  }
}
