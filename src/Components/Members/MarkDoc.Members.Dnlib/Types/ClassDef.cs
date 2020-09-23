using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using dnlib.DotNet;
using MarkDoc.Helpers;
using MarkDoc.Members.Dnlib.Members;
using MarkDoc.Members.Members;
using MarkDoc.Members.ResolvedTypes;
using MarkDoc.Members.Types;

namespace MarkDoc.Members.Dnlib.Types
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
    internal ClassDef(IResolver resolver, dnlib.DotNet.TypeDef source, dnlib.DotNet.TypeDef? parent)
      : base(resolver, source, parent, ResolveGenerics(resolver, source, parent), ResolveBaseClass(source, resolver, out var baseType))
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
                            && !methodDef.IsPrivate
                            && methodDef.IsConstructor)
        // Initialize constructors
        .Select(x => new ConstructorDef(resolver, x, parent != null))
        // Materialize the collection
        .ToReadOnlyCollection();

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

    private static IEnumerable<IResType> ResolveBaseClass(dnlib.DotNet.TypeDef source, IResolver resolver, out IResType? result)
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
