using System;
using System.Diagnostics;

namespace MarkDoc.Members.Dnlib
{
  [DebuggerDisplay("{Name}")]
  public class ResType
    : IResType
  {
    #region Properties

    /// <inheritdoc />
    public string Name { get; }

    /// <inheritdoc />
    public string TypeNamespace { get; }

    #endregion

    public ResType(dnlib.DotNet.IType source)
    {
      if (source == null)
        throw new ArgumentNullException(nameof(source));

      Name = ResolveName(source);
      TypeNamespace = source.Namespace;
    }

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
  }
}
