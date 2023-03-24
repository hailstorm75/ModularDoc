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
  /// Class for representing classes
  /// </summary>
  [DebuggerDisplay(nameof(ClassDef) + (": {" + nameof(Name) + "}"))]
  public class ClassDef
    : InterfaceDef, IClass
  {
    #region Properties

    /// <inheritdoc />
    public bool IsAbstract { get; }

    /// <inheritdoc />
    public IResType? BaseClass { get; }

    /// <inheritdoc />
    public IReadOnlyCollection<IConstructor> Constructors { get; }

    /// <inheritdoc />
    public IReadOnlyCollection<ModularDoc.Members.Members.IField> Fields { get; }

    /// <inheritdoc />
    public bool IsStatic { get; }

    /// <inheritdoc />
    public bool IsSealed { get; }

    #endregion

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="resolver">Type resolver instance</param>
    /// <param name="source">Type source</param>
    /// <param name="parent">Nested type parent</param>
    internal ClassDef(Resolver resolver, dnlib.DotNet.TypeDef source, dnlib.DotNet.TypeDef? parent, DotNetType type)
      : base(resolver, source, parent, ResolveGenerics(resolver, source, parent), ResolveBaseClass(source, resolver, out var baseType), type)
    {
      // If the source is null..
      if (source is null)
        // throw an exception
        throw new ArgumentNullException(nameof(source));

      // Initialize the base class
      BaseClass = baseType;

      // Initialize the constructors
      Constructors = source.Methods
        // Select valid constructors
        .Where(methodDef => !methodDef.SemanticsAttributes.HasFlag(MethodSemanticsAttributes.Getter)
                            && !methodDef.SemanticsAttributes.HasFlag(MethodSemanticsAttributes.Setter)
                            && (resolver.ProcessPrivate || !methodDef.IsPrivate)
                            && methodDef.IsConstructor)
        // Initialize constructors
        .Select(x => new ConstructorDef(resolver, x, parent != null))
        // Materialize the collection
        .ToReadOnlyCollection();

      if (resolver.ProcessFields)
        Fields = source.Fields
          .Where(field => (resolver.ProcessPrivate || !field.IsPrivate)
             && !field.Name.StartsWith("<"))
          .Select(field => new Members.FieldDef(resolver, field, parent != null))
          .ToReadOnlyCollection();
      else
        Fields = Array.Empty<ModularDoc.Members.Members.IField>();

      // Determine whether this type is static
      IsStatic = source.IsSealed && source.IsAbstract;

      // If the type is static..
      if (IsStatic)
      {
        // then it is not sealed
        IsSealed = false;
        // and not abstract
        IsAbstract = false;
      }
      // Otherwise..
      else
      {
        // then it can be sealed
        IsSealed = source.IsSealed;
        // and it can be abstract
        IsAbstract = source.IsAbstract;
      }
    }

    private static IEnumerable<IResType> ResolveBaseClass(dnlib.DotNet.TypeDef source, Resolver resolver, out IResType? result)
    {
      // If the base type is not an object..
      result = source.BaseType?.FullName.Equals("System.Object", StringComparison.InvariantCulture) == false
        // resolve the base type
        ? resolver.Resolve(source.BaseType.ToTypeSig(), source.ResolveTypeGenerics())
        // otherwise return a null base type
        : null;

      return result == null
        ? Enumerable.Empty<IResType>()
        : new[] { result };
    }
  }
}
