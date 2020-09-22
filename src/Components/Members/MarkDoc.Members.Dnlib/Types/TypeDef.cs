using System;
using dnlib.DotNet;
using MarkDoc.Members.Enums;
using IType = MarkDoc.Members.Types.IType;

namespace MarkDoc.Members.Dnlib.Types
{
  /// <summary>
  /// Base class for all types
  /// </summary>
  public abstract class TypeDef
    : IType
  {
    private static readonly char[] GENERIC_CHAR = { '`' };

    #region Properties

    /// <summary>
    /// Type resolver
    /// </summary>
    protected IResolver Resolver { get; }

    /// <inheritdoc />
    public string Name { get; }

    /// <inheritdoc />
    public AccessorType Accessor { get; }

    /// <inheritdoc />
    public string TypeNamespace { get; }

    /// <inheritdoc />
    public string RawName { get; }

    #endregion

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="resolver">Type resolver instance</param>
    /// <param name="source">Type source</param>
    /// <param name="parent">Nested type parent</param>
    protected internal TypeDef(IResolver resolver, dnlib.DotNet.TypeDef source, dnlib.DotNet.TypeDef? parent)
    {
      // If the source is null..
      if (source is null)
        // throw an exception
        throw new ArgumentNullException(nameof(source));

      // Initialize the accessor
      Accessor = ResolveAccessor(source);
      // Initialize the namespace
      TypeNamespace = ResolveNamespace(source);
      // Initialize the name
      Name = ResolveName(source, parent);
      // Initialize the raw name
      RawName = source.ReflectionFullName.Replace('+', '.');
      // Initialize the resolver
      Resolver = resolver;
    }

    #region Methods

    private static AccessorType ResolveAccessor(dnlib.DotNet.TypeDef type)
    {
      // If the type is public..
      if (type.Visibility == TypeAttributes.Public || (type.IsNested && type.Visibility == TypeAttributes.NestedPublic))
        // return public
        return AccessorType.Public;
      // If the type is nested protected
      if (type.Visibility == TypeAttributes.NestedFamily)
        // return protected
        return AccessorType.Protected;
      // If the type is nested protected internal
      if (type.Visibility == TypeAttributes.NestedFamORAssem)
        // return protected internal
        return AccessorType.ProtectedInternal;
      // Otherwise return internal
      return AccessorType.Internal;
    }

    private static string ResolveNamespace(dnlib.DotNet.TypeDef source)
    {
      // If the namespace is present..
      if (!string.IsNullOrEmpty(source.Namespace) && !source.IsNested)
        // return the namespace
        return source.Namespace;

      // Otherwise..

      // Get the source name
      var fullName = source.FullName.AsSpan();
      // Find the index of the nested delimiter
      var nestedIndex = fullName.IndexOf('/');
      // If the delimiter was found..
      if (nestedIndex != -1)
        // remove it
        fullName = fullName.Slice(0, nestedIndex);

      //// Find the last delimiter before the type name
      //var dotIndex = fullName.LastIndexOf('.');
      //// Remove the type name from the namespace
      //var result = fullName.Slice(0, dotIndex);

      // Return the result
      return fullName.ToString();
    }

    private static string ResolveName(dnlib.DotNet.IType source, IIsTypeOrMethod? parent)
    {
      ReadOnlySpan<char> CutNamespace(ReadOnlySpan<char> s, bool isNested)
      {
        // If the type is nested..
        if (isNested)
          // remove the parent and namespace
          return s.Slice(source.FullName.LastIndexOf('/') + 1);

        // If the namespace is present..
        return source.Namespace.Length != 0
          // remove it and return
          ? s.Slice(source.Namespace.Length + 1)
          // Otherwise return as is
          : s;
      }

      // Get the source name
      var fullName = source.FullName.AsSpan();

      // Remove the namespace
      var namespaceCut = CutNamespace(fullName, parent != null);
      // If the source has generics..
      if (source is ITypeOrMethodDef type && !type.HasGenericParameters)
        // return the result
        return namespaceCut.ToString();

      // Find the index of the generics
      var genericsIndex = namespaceCut.IndexOf(new ReadOnlySpan<char>(GENERIC_CHAR), StringComparison.InvariantCulture);
      // If there are no generics..
      if (genericsIndex == -1)
        // return the result
        return namespaceCut.ToString();
      // Remove the generics
      var genericCut = namespaceCut.Slice(0, genericsIndex);

      // Return the result
      return genericCut.ToString();
    }

    #endregion
  }
}
