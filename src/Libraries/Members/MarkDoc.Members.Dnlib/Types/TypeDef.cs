using System;
using dnlib.DotNet;
using MarkDoc.Members.Enums;
using IType = MarkDoc.Members.Types.IType;

namespace MarkDoc.Members.Dnlib.Types
{
  public abstract class TypeDef
    : IType
  {
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
    protected internal TypeDef(IResolver resolver, ITypeDefOrRef source, ITypeDefOrRef? parent)
    {
      if (source is null)
        throw new ArgumentNullException(nameof(source));

      // TODO: Assign accessor

      TypeNamespace = parent?.Namespace ?? source.Namespace;
      Name = ResolveName(source, parent);
      Resolver = resolver;
      RawName = source.ReflectionFullName.Replace('+', '.');
    }

    #region Methods

    private static string ResolveName(ITypeDefOrRef source, ITypeDefOrRef? parent)
    {
      var namespaceCut = CutNamespace(source, parent != null);
      if (source is ITypeOrMethodDef type && !type.HasGenericParameters)
        return namespaceCut;

      var genericsIndex = namespaceCut.IndexOf('`', StringComparison.InvariantCulture);
      if (genericsIndex == -1)
        return namespaceCut;
      var genericCut = namespaceCut.Remove(genericsIndex);

      return genericCut;
    } 

    private static string CutNamespace(ITypeDefOrRef source, bool isNested)
    {
      if (isNested)
        return source.FullName.Substring(source.FullName.LastIndexOf('/') + 1);

      return source.Namespace.Length != 0
        ? source.FullName.Substring(source.Namespace.Length + 1)
        : source.FullName;
    }

    #endregion
  }
}
