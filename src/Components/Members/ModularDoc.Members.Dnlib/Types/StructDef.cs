using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using dnlib.DotNet;
using ModularDoc.Helpers;
using ModularDoc.Members.Dnlib.Helpers;
using ModularDoc.Members.Dnlib.Members;
using ModularDoc.Members.Enums;
using ModularDoc.Members.Members;
using ModularDoc.Members.ResolvedTypes;
using ModularDoc.Members.Types;

namespace ModularDoc.Members.Dnlib.Types
{
  /// <summary>
  /// Class for representing structures
  /// </summary>
  [DebuggerDisplay(nameof(StructDef) + ": {" + nameof(Name) + "}")]
  public class StructDef
    : InterfaceDef, IStruct
  {
    #region Properties

    /// <inheritdoc />
    public IReadOnlyCollection<IConstructor> Constructors { get; }

    /// <inheritdoc />
    public bool IsReadOnly { get; }

    #endregion

    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="resolver">Type resolver instance</param>
    /// <param name="source">Type source</param>
    /// <param name="parent">Nested type parent</param>
    internal StructDef(Resolver resolver, dnlib.DotNet.TypeDef source, dnlib.DotNet.TypeDef? parent, DotNetType type)
      : base(resolver, source, parent, ResolveGenericStructs(resolver, source, parent), Enumerable.Empty<IResType>(), type)
    {
      IsReadOnly = source
        .CustomAttributes
        .Any(x => x.TypeFullName.Equals("System.Runtime.CompilerServices.IsReadOnlyAttribute", StringComparison.InvariantCultureIgnoreCase));
      // Initialize the constructors
      Constructors = source.Methods
        // Select valid constructors
        .Where(methodDef => !methodDef.SemanticsAttributes.HasFlag(MethodSemanticsAttributes.Getter)
                            && !methodDef.SemanticsAttributes.HasFlag(MethodSemanticsAttributes.Setter)
                            && !methodDef.IsPrivate
                            && methodDef.IsConstructor)
        // Initialize constructors
        .Select(x => new ConstructorDef(resolver, x, parent != null))
        // Materialize the collection
        .ToReadOnlyCollection();
    }

    #endregion

    private static IReadOnlyDictionary<string, (Variance variance, IReadOnlyCollection<IResType>)> ResolveGenericStructs(Resolver resolver, dnlib.DotNet.TypeDef source, dnlib.DotNet.TypeDef? parent)
    {
      IResType ResolveType(GenericParamConstraint x, IReadOnlyDictionary<string, string> generics)
        => resolver.Resolve(x.Constraint.ToTypeSig(), generics);

      (Variance, IReadOnlyCollection<IResType>) ResolveParameter(GenericParam parameter, IReadOnlyDictionary<string, string> generics)
      {
        // If the parameter has no generic constraints..
        if (!parameter.HasGenericParamConstraints)
          // return default
          return (Variance.NonVariant, Enumerable.Empty<IResType>().ToArray());

        // Otherwise return the list of constraints
        return (Variance.NonVariant, parameter.GenericParamConstraints.Select(x => ResolveType(x, generics)).ToReadOnlyCollection());
      }

      return source.GenericParameters
        // Exclude generic parameters from the parent
        .Except(parent?.GenericParameters ?? Enumerable.Empty<GenericParam>(), EqualityComparerEx<GenericParam>.Create(x => x.Name, x => x.Name))
        // Materialize the result as a dictionary
        .ToDictionary(param => param.Name.String, param => ResolveParameter(param, source.ResolveTypeGenerics()));
    }
  }
}
