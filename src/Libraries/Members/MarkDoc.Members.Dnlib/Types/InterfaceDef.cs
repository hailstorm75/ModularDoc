using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using dnlib.DotNet;
using MarkDoc.Helpers;
using MarkDoc.Members.Dnlib.Members;
using MarkDoc.Members.Dnlib.Properties;
using MarkDoc.Members.Enums;
using MarkDoc.Members.Members;
using MarkDoc.Members.ResolvedTypes;
using MarkDoc.Members.Types;
using EventDef = MarkDoc.Members.Dnlib.Members.EventDef;
using IMethod = MarkDoc.Members.Members.IMethod;
using IType = MarkDoc.Members.Types.IType;
using MethodDef = MarkDoc.Members.Dnlib.Members.MethodDef;
using PropertyDef = MarkDoc.Members.Dnlib.Members.PropertyDef;

namespace MarkDoc.Members.Dnlib.Types
{
  [DebuggerDisplay(nameof(InterfaceDef) + (": {" + nameof(Name) + "}"))]
  public class InterfaceDef
    : TypeDef, IInterface
  {
    #region Properties

    /// <inheritdoc />
    public IReadOnlyCollection<IResType> InheritedInterfaces { get; }
    /// <inheritdoc />
    public IReadOnlyDictionary<string, (Variance variance, IReadOnlyCollection<IResType> constraints)> Generics { get; }

    /// <inheritdoc />
    public IReadOnlyCollection<IDelegate> Delegates { get; }

    /// <inheritdoc />
    public IReadOnlyCollection<IType> NestedTypes { get; }

    /// <inheritdoc />
    public IReadOnlyCollection<IEvent> Events { get; }

    /// <inheritdoc />
    public IReadOnlyCollection<IMethod> Methods { get; }

    /// <inheritdoc />
    public IReadOnlyCollection<IProperty> Properties { get; }

    /// <inheritdoc />
    public Lazy<IReadOnlyDictionary<IMember, IInterface>> InheritedTypes { get; }

    #endregion

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="resolver">Type resolver instance</param>
    /// <param name="source">Type source</param>
    /// <param name="parent">Nested type parent</param>
    internal InterfaceDef(IResolver resolver, dnlib.DotNet.TypeDef source, dnlib.DotNet.TypeDef? parent)
      : this(resolver, source, parent, ResolveGenerics(resolver, source, parent), Enumerable.Empty<IResType>()) { }

    protected InterfaceDef(IResolver resolver, dnlib.DotNet.TypeDef source, dnlib.DotNet.TypeDef? parent, IReadOnlyDictionary<string, (Variance variance, IReadOnlyCollection<IResType>)> generics, IEnumerable<IResType> inheritedTypes)
      : base(resolver, source, parent)
    {
      if (source is null)
        throw new ArgumentNullException(nameof(source));

      InheritedInterfaces = ResolveInterfaces(source, source.ResolveTypeGenerics()).ToReadOnlyCollection();
      Generics = generics;
      Delegates = source.NestedTypes.Where(x => x.IsDelegate)
                                    .Select(x => new DelegateDef(resolver, x))
                                    .ToReadOnlyCollection();
      NestedTypes = source.NestedTypes.Where(x => !x.IsDelegate && !x.Name.String.StartsWith('<'))
                                      .Select(x => Resolver.ResolveType(x, source))
                                      .ToReadOnlyCollection();
      Methods = source.Methods.Where(x => !x.SemanticsAttributes.HasFlag(MethodSemanticsAttributes.Getter)
                                       && !x.SemanticsAttributes.HasFlag(MethodSemanticsAttributes.Setter)
                                       && !x.Access.HasFlag(MethodAttributes.Assembly)
                                       && !x.IsPrivate
                                       && !x.IsConstructor)
                              .Select(x => new MethodDef(resolver, x))
                              .ToReadOnlyCollection();
      Properties = source.Properties.Select(x => PropertyDef.Initialize(resolver, x))
                                    .WhereNotNull()
                                    .ToReadOnlyCollection();
      Events = source.Events.Select(x => new EventDef(resolver, x))
                            .ToReadOnlyCollection();
      InheritedTypes = new Lazy<IReadOnlyDictionary<IMember, IInterface>>(() => ResolveInheritedMembers(InheritedInterfaces.Concat(inheritedTypes)), LazyThreadSafetyMode.PublicationOnly);
    }

    #region Methods

    private IReadOnlyDictionary<IMember, IInterface> ResolveInheritedMembers(IEnumerable<IResType> inheritedTypes)
    {
      IEnumerable<(IMember member, IInterface type)> GetInheritedTypes(IInterface type)
      {
        IEnumerable<(IMember member, IInterface type)> Intersect<T>(IEnumerable<T> left, IEnumerable<T> right)
          where T : IMember
          => left.Intersect(right, EqualityComparerEx<T>.Create(x => x.RawName, x=> x.RawName)).Select(x => (x as IMember, type));

        var methods = Intersect(type.Methods, Methods);
        var properties = Intersect(type.Properties, Properties);
        var events = Intersect(type.Events, Events);
        var delegates = Intersect(type.Delegates, Delegates);

        return methods
          .Concat(properties)
          .Concat(events)
          .Concat(delegates)
          .Where(x => !type.InheritedTypes.Value.ContainsKey(x.member))
          .Concat(type.InheritedTypes.Value.Select(x => (x.Key, x.Value)));
      }

      var res = inheritedTypes
        .Select(x => x.Reference.Value)
        .WhereNotNull()
        .OfType<IInterface>()
        .Select(GetInheritedTypes)
        .SelectMany(Linq.XtoX).ToArray();

      var b = res.GroupBy(x => x.member).ToDictionary(Linq.GroupKey, x => x.GroupValues().ToArray());

      return res.ToDictionary(x => x.member, x => x.type);
    }

    private IEnumerable<IResType> ResolveInterfaces(dnlib.DotNet.TypeDef source, IReadOnlyDictionary<string, string> outerArgs)
      => source.Interfaces.Select(x => Resolver.Resolve(x.Interface.ToTypeSig(), outerArgs));

    protected static IReadOnlyDictionary<string, (Variance variance, IReadOnlyCollection<IResType>)> ResolveGenerics(IResolver resolver, dnlib.DotNet.TypeDef source, dnlib.DotNet.TypeDef? parent)
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

      if (source is null)
        throw new ArgumentNullException(nameof(source));

      return source.GenericParameters.Except(parent?.GenericParameters ?? Enumerable.Empty<GenericParam>(), EqualityComparerEx<GenericParam>.Create(x => x.Name, x => x.Name))
                                     .ToDictionary(x => x.Name.String, x => ResolveParameter(x, source.ResolveTypeGenerics()));
    }

    #endregion
  }
}
