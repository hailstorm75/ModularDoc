using dnlib.DotNet;
using System;
using System.Diagnostics;
using System.Threading;

namespace MarkDoc.Members.Dnlib
{
  [DebuggerDisplay("{DisplayName}")]
  public class ResType
    : IResType
  {
    #region Properties

    protected IResolver Resolver { get; }

    /// <inheritdoc />
    public string Name { get; }

    /// <inheritdoc />
    public string TypeNamespace { get; }

    /// <inheritdoc />
    public string DisplayName { get; }

    /// <inheritdoc />
    public Lazy<IType?> Reference { get; }

    #endregion

    internal ResType(IResolver resolver, dnlib.DotNet.TypeSig source)
      : this(resolver, source, ResolveName(source)) { }

    internal protected ResType(IResolver resolver, dnlib.DotNet.TypeSig source, string displayName)
    {
      if (source == null)
        throw new ArgumentNullException(nameof(source));

      Resolver = resolver;
      Name = ResolveName(source);
      DisplayName = displayName;
      TypeNamespace = source.Namespace;
      Reference = new Lazy<IType?>(() => Resolver.FindReference(source, this), LazyThreadSafetyMode.ExecutionAndPublication);
    }

    #region Methods

    protected static string ResolveName(dnlib.DotNet.IType source)
    {
      if (source == null)
        throw new ArgumentNullException(nameof(source));

      var name = source.Name.String;
      var genericsIndex = name.IndexOf('`', StringComparison.InvariantCulture);
      if (genericsIndex == -1)
        return name;

      return name.Remove(genericsIndex);
    } 

    #endregion
  }
}
