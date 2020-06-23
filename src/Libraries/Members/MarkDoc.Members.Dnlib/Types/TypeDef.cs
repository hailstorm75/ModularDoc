using System;
using dnlib.DotNet;
using MarkDoc.Members.Enums;
using IType = MarkDoc.Members.Types.IType;

namespace MarkDoc.Members.Dnlib.Types
{
  public abstract class TypeDef
    : IType
  {
    private static readonly char[] GENERIC_CHAR = new[] { '`' };

    #region Properties

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
    protected internal TypeDef(IResolver resolver, dnlib.DotNet.TypeDef source, ITypeDefOrRef? parent)
    {
      if (source is null)
        throw new ArgumentNullException(nameof(source));

      Accessor = ResolveAccessor(source);
      TypeNamespace = parent?.Namespace ?? source.Namespace;
      Name = ResolveName(source, parent);
      Resolver = resolver;
      RawName = source.ReflectionFullName.Replace('+', '.');
    }

    #region Methods

    private static AccessorType ResolveAccessor(dnlib.DotNet.TypeDef @delegate)
    {
      if (@delegate.Visibility == TypeAttributes.Public)
        return AccessorType.Public;
      if (@delegate.Visibility == TypeAttributes.NestedFamily)
        return AccessorType.Protected;
      return AccessorType.Internal;
    }

    private static string ResolveName(dnlib.DotNet.IType source, IIsTypeOrMethod? parent)
    {
      ReadOnlySpan<char> CutNamespace(ReadOnlySpan<char> s, bool isNested)
      {
        if (isNested)
          return s.Slice(source.FullName.LastIndexOf('/') + 1);

        return source.Namespace.Length != 0
          ? s.Slice(source.Namespace.Length + 1)
          : s;
      }

      var fullName = source.FullName.AsSpan();

      var namespaceCut = CutNamespace(fullName, parent != null);
      if (source is ITypeOrMethodDef type && !type.HasGenericParameters)
        return namespaceCut.ToString();

      var genericsIndex = namespaceCut.IndexOf(new ReadOnlySpan<char>(GENERIC_CHAR), StringComparison.InvariantCulture);
      if (genericsIndex == -1)
        return namespaceCut.ToString();
      var genericCut = namespaceCut.Slice(0, genericsIndex);

      return genericCut.ToString();
    }

    #endregion
  }
}
