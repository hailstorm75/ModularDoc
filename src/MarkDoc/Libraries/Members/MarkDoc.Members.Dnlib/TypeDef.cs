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
    protected TypeDef(dnlib.DotNet.TypeDef source)
    {
      if (source == null)
        throw new ArgumentNullException(nameof(source));

      TypeNamespace = source.Namespace;
      Name = ResolveName(source);
    }

    private static string ResolveName(dnlib.DotNet.TypeDef source)
    {
      var namespaceCut = source.FullName.Substring(source.Namespace.Length + 1);

      if (!source.HasGenericParameters)
        return namespaceCut;

      var genericCut = namespaceCut.Substring(0, namespaceCut.IndexOf('`', StringComparison.InvariantCulture));

      return genericCut;
    }
  }
}
