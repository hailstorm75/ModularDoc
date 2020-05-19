using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using dnlib.DotNet;
using MarkDoc.Members.ResolvedTypes;

namespace MarkDoc.Members.Dnlib.ResolvedTypes
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

    internal ResType(IResolver resolver, TypeSig source)
      : this(resolver, source, ResolveName(source), ResolveRawName(source)) { }

    protected ResType(IResolver resolver, TypeSig source, string displayName, string rawNamee)
    {
      if (source == null)
        throw new ArgumentNullException(nameof(source));

      Resolver = resolver;
      Name = rawNamee;
      DisplayName = displayName;
      TypeNamespace = source.Namespace;
      Reference = new Lazy<IType?>(() => Resolver.FindReference(source, this), LazyThreadSafetyMode.ExecutionAndPublication);
    }

    #region Methods

    private static string ResolveRawName(TypeSig source)
    {
      static IEnumerable<string> RetrieveNested(ITypeDefOrRef source)
      {
        static string SolveGenerics(string value)
        {
          var index = value.IndexOf('`', StringComparison.InvariantCultureIgnoreCase);
          if (index == -1)
            return value;

          var name = value.Remove(index);
          if (!int.TryParse(value.Substring(index + 1), out var number))
            return name;
          return $"{name}{{{string.Join(",", Enumerable.Repeat('`', number).Select((x,i) => $"{x}{i}"))}}}";
        }

        if (source == null)
          yield break;
        var type = RetrieveNested(source.ScopeType.DeclaringType);
        foreach (var item in type)
          yield return item;

        if (source.ScopeType.DeclaringType == null)
          yield return source.Namespace;

        yield return SolveGenerics(source.Name);
      }

      if (!source.FullName.Contains('/', StringComparison.InvariantCultureIgnoreCase))
        return source.FullName;

      var result = $"{string.Join(".", RetrieveNested(source.ScopeType))}";
      return result;
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

    #endregion
  }
}
