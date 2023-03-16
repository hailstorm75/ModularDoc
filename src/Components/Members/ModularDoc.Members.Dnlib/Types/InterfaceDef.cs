using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using dnlib.DotNet;
using ModularDoc.Helpers;
using ModularDoc.Members.Dnlib.Helpers;
using ModularDoc.Members.Dnlib.Members;
using ModularDoc.Members.Dnlib.Properties;
using ModularDoc.Members.Enums;
using ModularDoc.Members.Members;
using ModularDoc.Members.ResolvedTypes;
using ModularDoc.Members.Types;
using EventDef = ModularDoc.Members.Dnlib.Members.EventDef;
using IMethod = ModularDoc.Members.Members.IMethod;
using IType = ModularDoc.Members.Types.IType;
using MethodDef = ModularDoc.Members.Dnlib.Members.MethodDef;
using PropertyDef = ModularDoc.Members.Dnlib.Members.PropertyDef;

namespace ModularDoc.Members.Dnlib.Types
{
  /// <summary>
  /// Class for representing interfaces
  /// </summary>
  [DebuggerDisplay(nameof(InterfaceDef) + (": {" + nameof(Name) + "}"))]
  public class InterfaceDef
    : TypeDef, IInterface
  {
    #region Properties

    /// <inheritdoc />
    public IReadOnlyCollection<IResType> InheritedTypesFlat { get; }

    /// <inheritdoc />
    public Lazy<IReadOnlyCollection<TreeNode>> InheritedTypesStructured { get; }

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
    public Lazy<IReadOnlyDictionary<IMember, IInterface>> InheritedTypeMembers { get; }

    #endregion

    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="resolver">Type resolver instance</param>
    /// <param name="source">Type source</param>
    /// <param name="parent">Nested type parent</param>
    internal InterfaceDef(Resolver resolver, dnlib.DotNet.TypeDef source, dnlib.DotNet.TypeDef? parent, DotNetType type)
      : this(resolver, source, parent, ResolveGenerics(resolver, source, parent), Enumerable.Empty<IResType>(), type) { }

    /// <summary>
    /// Constructor for derived types
    /// </summary>
    /// <param name="resolver">Type resolver instance</param>
    /// <param name="source">Type source</param>
    /// <param name="parent">Nested type parent</param>
    /// <param name="generics">Generics of this given type</param>
    /// <param name="inheritedTypes">Inherited types</param>
    protected InterfaceDef(Resolver resolver, dnlib.DotNet.TypeDef source, dnlib.DotNet.TypeDef? parent, IReadOnlyDictionary<string, (Variance variance, IReadOnlyCollection<IResType>)> generics, IEnumerable<IResType> inheritedTypes, DotNetType type)
      : base(resolver, source, parent, type)
    {
      // If the source is null..
      if (source is null)
        // throw an exception
        throw new ArgumentNullException(nameof(source));

      // Initialize the generics
      Generics = generics;

      // Initialize the inherited types flattened
      InheritedTypesFlat = ResolveInheritedTypesFlat(source, source.ResolveTypeGenerics()).ToReadOnlyCollection();

      // Initialize the inherited types structured
      InheritedTypesStructured = new Lazy<IReadOnlyCollection<TreeNode>>(ResolveInheritedTypesStructured, LazyThreadSafetyMode.PublicationOnly);

      // Initialize the delegates
      Delegates = source.NestedTypes
        // Select types which are delegates
        .Where(x => x.IsDelegate)
        // Initialize delegates
        .Select(x => new DelegateDef(resolver, x))
        // Materialize the collection
        .ToReadOnlyCollection();

      // Initialize the nested types
      NestedTypes = source.NestedTypes
        // Select types which are valid nested types
        .Where(typeDef => !typeDef.IsDelegate
                          && (resolver.ProcessPrivate || !typeDef.IsNestedPrivate)
                          && !typeDef.Name.String.StartsWith('<'))
        // Resolve the nested types
        .Select(typeDef => Resolver.ResolveType(typeDef, source))
        // Materialize the collection
        .ToReadOnlyCollection();

      // Initialize the events
      Events = source.Events
        // Initialize events
        .Select(eventDef => new EventDef(resolver, eventDef))
        // Materialize the collection
        .ToReadOnlyCollection();

      var eventMethods = GetEventMembers(Events).ToHashSet();

      // Initialize the methods
      Methods = source.Methods
        // Select members which are non-private methods
        .Where(methodDef => !methodDef.SemanticsAttributes.HasFlag(MethodSemanticsAttributes.Getter)
                            && !methodDef.SemanticsAttributes.HasFlag(MethodSemanticsAttributes.Setter)
                            && (resolver.ProcessPrivate || !methodDef.IsPrivate)
                            && !methodDef.IsConstructor
                            && !eventMethods.Contains(methodDef.Name.String))
        // Initialize methods
        .Select(methodDef => new MethodDef(resolver, methodDef))
        // Materialize the collection
        .ToReadOnlyCollection();

      // Initialize the properties
      Properties = source.Properties
        // Initialize properties
        .Select(propertyDef => PropertyDef.Initialize(resolver, propertyDef))
        // Filter out invalid properties
        .WhereNotNull()
        // Materialize the collection
        .ToReadOnlyCollection();

      // Initialize inherited members
      InheritedTypeMembers = new Lazy<IReadOnlyDictionary<IMember, IInterface>>(() => ResolveInheritedMembers(InheritedTypesFlat.Concat(inheritedTypes)), LazyThreadSafetyMode.PublicationOnly);
    }

    #endregion

    #region Methods

    private static IEnumerable<string> GetEventMembers(IEnumerable<IEvent> events)
    {
      foreach (var @event in events)
      {
        yield return $"add_{@event.Name}";
        yield return $"remove_{@event.Name}";
      }
    }

    private IReadOnlyDictionary<IMember, IInterface> ResolveInheritedMembers(IEnumerable<IResType> inheritedTypes)
    {
      IEnumerable<(IMember member, IInterface type)> GetInheritedTypes(IInterface type)
      {
        //IEnumerable<(IMember member, IInterface type)> Intersect<T>(IEnumerable<T> left, IEnumerable<T> right)
        //  where T : IMember
        //  => left
        //      // Intersect by raw names
        //      .Intersect(right, EqualityComparerEx<T>.Create(x => x.RawName, x=> x.RawName))
        //      // Select the inherited member paired with the source type
        //      .Select(member => (member as IMember, type));

        // Select inherited methods
        IEnumerable<(IMember member, IInterface type)> methods = type.Methods.Select(member => (member as IMember, type));
        // Select inherited properties
        IEnumerable<(IMember member, IInterface type)> properties = type.Properties.Select(member => (member as IMember, type));
        // Select inherited events
        IEnumerable<(IMember member, IInterface type)> events = type.Events.Select(member => (member as IMember, type));
        // Select inherited delegates
        IEnumerable<(IMember member, IInterface type)> delegates = type.Delegates.Select(member => (member as IMember, type));

        return methods
          .Concat(properties)
          .Concat(events)
          .Concat(delegates)
          // Select unique inherited types
          .Where(x => !type.InheritedTypeMembers.Value.ContainsKey(x.member))
          // Join the newly resolved inherited types with the parent inherited types
          .Concat(type.InheritedTypeMembers.Value.Select(x => (x.Key, x.Value)));
      }

      return inheritedTypes
        // Select type references
        .Select(type => type.Reference.Value)
        // Filter out types which are missing references
        .WhereNotNull()
        // Select interface types
        .OfType<IInterface>()
        // Get inherited types
        .Select(GetInheritedTypes)
        // Flatten the collection
        .SelectMany(Linq.XtoX)
        // Ensure there are no duplicates
        .DistinctBy(Linq.XtoX)
        // Materialize the collection as a dictionary
        .ToDictionary(x => x.member, x => x.type);
    }

    private IEnumerable<IResType> ResolveInheritedTypesFlat(dnlib.DotNet.TypeDef source, IReadOnlyDictionary<string, string> outerArgs)
      => source.Interfaces.Select(interfaceImpl => Resolver.Resolve(interfaceImpl.Interface.ToTypeSig(), outerArgs));

    private IReadOnlyCollection<TreeNode> ResolveInheritedTypesStructured()
    {
      // Prepare a list of sub-inherited type raw names by the types inherited by this type
      var subFlatList = new HashSet<string>(InheritedTypesFlat.Count);
      // For each inherited type by this type..
      foreach (var item in InheritedTypesFlat)
      {
        // If it is part of the sub-inherited types or it is not part of the resolved set of types..
        if (subFlatList.Contains(item.RawName) || item.Reference.Value is null)
          // Skip this inherited type
          continue;

        // If the given resolved type is not at least an interface..
        if (item.Reference.Value is not IInterface inter)
          // Then it is not an inheritable type and an exception must be thrown
          throw new NotSupportedException("An inherited type is not at least an interface");

        // Populate the list of sub-inherited types
        subFlatList.AddRange(inter.InheritedTypesFlat.Select(type => type.RawName));
      }

      // Return a structured list of inherited types
      return InheritedTypesFlat
        // Skip the types that originate from the sub-inherited types
        .Where(type => !subFlatList.Contains(type.RawName))
        // Select the structured tree nodes
        .Select(type =>
        {
          // Extract a list of tree nodes from the inherited types
          var children = type.Reference.Value is IInterface i
            ? i.InheritedTypesStructured.Value
            : Array.Empty<TreeNode>();
          // Return a materialized tree node
          return new TreeNode(type.RawName, type, children);
        })
        // Materialize the list
        .ToReadOnlyCollection();
    }

    protected static IReadOnlyDictionary<string, (Variance variance, IReadOnlyCollection<IResType>)> ResolveGenerics(Resolver resolver, dnlib.DotNet.TypeDef source, dnlib.DotNet.TypeDef? parent)
    {
      IResType ResolveType(GenericParamConstraint x, IReadOnlyDictionary<string, string> generics)
        => resolver.Resolve(x.Constraint.ToTypeSig(), generics);

      (Variance, IReadOnlyCollection<IResType>) ResolveParameter(GenericParam parameter, IReadOnlyDictionary<string, string> generics)
      {
        static Variance ResolveVariance(GenericParamAttributes attributes)
          => attributes switch
          {
            GenericParamAttributes.NonVariant => Variance.NonVariant,
            GenericParamAttributes.Covariant => Variance.Covariant,
            GenericParamAttributes.Contravariant => Variance.Contravariant,
            _ => throw new NotSupportedException(Resources.varianceInvalid),
          };

        // Retrieve the parameter variance
        var variance = ResolveVariance(parameter.Variance);

        // If there are no generic constraints
        return !parameter.HasGenericParamConstraints
          // return the default
          ? (variance, Enumerable.Empty<IResType>().ToArray())
          // otherwise return the generic constraints
          : (variance, parameter.GenericParamConstraints.Select(constraint => ResolveType(constraint, generics)).ToReadOnlyCollection());
      }


      // If the source is null..
      if (source is null)
        // throw an exception
        throw new ArgumentNullException(nameof(source));

      return source.GenericParameters
        // Remove the generic parameters inherited from the parent type
        .Except(parent?.GenericParameters ?? Enumerable.Empty<GenericParam>(), EqualityComparerEx<GenericParam>.Create(x => x.Name, x => x.Name))
        // Materialize the collection as a dictionary
        .ToDictionary(x => x.Name.String, x => ResolveParameter(x, source.ResolveTypeGenerics()));
    }

    #endregion
  }
}
