using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using dnlib.DotNet;
using MarkDoc.Helpers;
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
  [DebuggerDisplay(nameof(StructDef) + ": {" + nameof(Name) + "}")]
  public class StructDef
    : TypeDef, IStruct
  {
    #region Properties

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

    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="resolver">Type resolver instance</param>
    /// <param name="source">Type source</param>
    /// <param name="parent">Nested type parent</param>
    internal StructDef(IResolver resolver, dnlib.DotNet.TypeDef source, dnlib.DotNet.TypeDef? parent)
      : this(resolver, source, parent, ResolveGenerics(resolver, source, parent)) { }

    /// <summary>
    /// Inherited constructor
    /// </summary>
    /// <param name="resolver">Type resolver instance</param>
    /// <param name="source">Type source</param>
    /// <param name="parent">Nested type parent</param>
    /// <param name="generics">Generic types to initialize with</param>
    protected StructDef(IResolver resolver, dnlib.DotNet.TypeDef source, dnlib.DotNet.TypeDef? parent, IReadOnlyDictionary<string, (Variance variance, IReadOnlyCollection<IResType> constraints)> generics)
      : base(resolver, source, parent)
    {
      if (source is null)
        throw new ArgumentNullException(nameof(source));

      Generics = generics;
      NestedTypes = source.NestedTypes.Where(x => !x.Name.String.StartsWith('<'))
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
    }

    #endregion

    private static IReadOnlyDictionary<string, (Variance variance, IReadOnlyCollection<IResType>)> ResolveGenerics(IResolver resolver, dnlib.DotNet.TypeDef source, dnlib.DotNet.TypeDef? parent)
    {
      IResType ResolveType(GenericParamConstraint x, IReadOnlyDictionary<string, string> generics)
        => resolver.Resolve(x.Constraint.ToTypeSig(), generics) ?? throw new Exception();

      (Variance, IReadOnlyCollection<IResType>) ResolveParameter(GenericParam parameter, IReadOnlyDictionary<string, string> generics)
      {
        var variance = Variance.NonVariant;

        if (!parameter.HasGenericParamConstraints)
          return (variance, Enumerable.Empty<IResType>().ToArray());

        return (variance, parameter.GenericParamConstraints.Select(x => ResolveType(x, generics)).ToReadOnlyCollection());
      }

      return source.GenericParameters.Except(parent?.GenericParameters ?? Enumerable.Empty<GenericParam>(), EqualityComparerEx<GenericParam>.Create(x => x.Name, x => x.Name))
                                      .ToDictionary(x => x.Name.String, x => ResolveParameter(x, source.ResolveTypeGenerics()));
    }
  }
}
