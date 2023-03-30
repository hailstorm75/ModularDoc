using System;
using System.Collections.Generic;
using System.Linq;
using dnlib.DotNet;
using ModularDoc.Helpers;
using ModularDoc.Members.Dnlib.Helpers;
using ModularDoc.Members.Enums;
using ModularDoc.Members.ResolvedTypes;
using IType = ModularDoc.Members.Types.IType;

namespace ModularDoc.Members.Dnlib.Types
{
  /// <summary>
  /// Base class for all types
  /// </summary>
  public abstract class TypeDef
    : IType
  {
    #region Fields

    private static readonly char[] GENERIC_CHAR = { '`' };

    #endregion

    #region Properties

    /// <summary>
    /// Type resolver
    /// </summary>
    protected Resolver Resolver { get; }

    /// <inheritdoc />
    public string Name { get; }

    /// <inheritdoc />
    public bool IsNested { get; }

    /// <inheritdoc />
    public AccessorType Accessor { get; }

    /// <inheritdoc />
    public DotNetType Type { get; }

    /// <inheritdoc />
    public string TypeNamespace { get; }

    /// <inheritdoc />
    public string RawName { get; }

    /// <inheritdoc />
    public IReadOnlyCollection<IResAttribute> Attributes { get; }

    #endregion

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="resolver">Type resolver instance</param>
    /// <param name="source">Type source</param>
    /// <param name="parent">Nested type parent</param>
    protected internal TypeDef(Resolver resolver, dnlib.DotNet.TypeDef source, dnlib.DotNet.TypeDef? parent, DotNetType type)
    {
      // If the source is null..
      if (source is null)
        // throw an exception
        throw new ArgumentNullException(nameof(source));

      var fileAccessorMatch = RegexHelpers.FILE_ACCESSOR_REGEX.Match(source.Name);

      // Initialize the accessor
      Accessor = fileAccessorMatch.Success
        ? AccessorType.File
        : ResolveAccessor(source);
      // Initialize the namespace
      TypeNamespace = ResolveNamespace(source);
      // Initialize the name
      Name = fileAccessorMatch.Success
        ? fileAccessorMatch.Groups["typeName"].Value
        : ResolveName(source, parent);
      // Initialize the raw name
      RawName = source.ReflectionFullName.Replace('+', '.');
      // Initialize the resolver
      Resolver = resolver;
      // Initialize the is nested indicator
      IsNested = parent is not null;

      Attributes = source.CustomAttributes
        .Select(resolver.ResolveAttribute)
        .ToReadOnlyCollection();

      Type = type;
    }

    #region Methods

    private static AccessorType ResolveAccessor(dnlib.DotNet.TypeDef type)
    {
      return type.Visibility switch
      {
        // If the type is public..
        TypeAttributes.Public => AccessorType.Public,
        // If the type is nested public..
        TypeAttributes.NestedPublic => AccessorType.Public,
        // If the type is nested protected
        TypeAttributes.NestedFamily => AccessorType.Protected,
        // If the type is nested protected internal
        TypeAttributes.NestedFamORAssem => AccessorType.ProtectedInternal,
        // If the type is nested private
        TypeAttributes.NestedPrivate => AccessorType.Private,
        // Otherwise return internal
        _ => AccessorType.Internal
      };
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
