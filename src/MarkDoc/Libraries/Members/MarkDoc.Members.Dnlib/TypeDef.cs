using dnlib.DotNet;
using MarkDoc.Members.Enums;
using System;

namespace MarkDoc.Members.Dnlib
{
  public abstract class TypeDef
    : IType
  {
    #region Properties

    /// <inheritdoc />
    public bool IsStatic { get; }

    /// <inheritdoc />
    public string Name { get; }

    /// <inheritdoc />
    public AccessorType Accessor { get; }

    /// <inheritdoc />
    public string TypeNamespace { get; }

    #endregion

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="source">Type source</param>
    /// <param name="parent">Nested type parent</param>
    protected TypeDef(dnlib.DotNet.ITypeDefOrRef source, dnlib.DotNet.ITypeDefOrRef? parent)
    {
      if (source == null)
        throw new ArgumentNullException(nameof(source));

      TypeNamespace = parent?.Namespace ?? source.Namespace;
      Name = ResolveName(source, parent);
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
