using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using MarkDoc.Members.ResolvedTypes;
using MarkDoc.Members.Types;

namespace MarkDoc.Members.Dnlib.ResolvedTypes
{
  public class ResArray
    : IResArray, IResType
  {
    #region Properties

    private IResolver Resolver { get; }

    /// <inheritdoc />
    public IResType ArrayType { get; }

    /// <inheritdoc />
    public bool IsJagged { get; }

    /// <inheritdoc />
    public int Dimension { get; }

    /// <inheritdoc />
    public string DisplayName
      => ArrayType.DisplayName;

    /// <inheritdoc />
    public string DocumentationName
    {
      get
      {
        if (IsJagged)
          return ArrayType.DocumentationName + string.Join(string.Empty, Enumerable.Repeat("[]", Dimension));
        return ArrayType.DocumentationName + $"[{string.Concat(Enumerable.Repeat(",", Dimension - 1))}]";
      }
    }

    /// <inheritdoc />
    public string TypeNamespace
      => ArrayType.TypeNamespace;

    /// <inheritdoc />
    public Lazy<IType?> Reference { get; }

    /// <inheritdoc />
    public string RawName { get; }

    #endregion

    internal ResArray(IResolver resolver, dnlib.DotNet.TypeSig source, IReadOnlyDictionary<string, string>? generics)
    {
      if (source is null)
        throw new ArgumentNullException(nameof(source));
      if (resolver is null)
        throw new ArgumentNullException(nameof(resolver));

      RawName = source.FullName;
      IsJagged = source.ElementType == dnlib.DotNet.ElementType.SZArray;
      Resolver = resolver;

      var next = ResolveNext(source, IsJagged);
      ArrayType = Resolver.Resolve(next, generics);
      Dimension = ResolveDimension(source, next);
      Reference = new Lazy<IType?>(() => Resolver.FindReference(source, this), LazyThreadSafetyMode.PublicationOnly);
    }

    #region Methods

    private static dnlib.DotNet.TypeSig ResolveNext(dnlib.DotNet.TypeSig source, bool isJagged)
    {
      dnlib.DotNet.TypeSig? next = source.Next;
      dnlib.DotNet.TypeSig current = source;

      while (next?.ElementType == (isJagged ? dnlib.DotNet.ElementType.SZArray : dnlib.DotNet.ElementType.Array))
      {
        current = next;
        next = current.Next;
      }

      return next ?? current;
    }

    private int ResolveDimension(dnlib.DotNet.TypeSig source, dnlib.DotNet.TypeSig next)
    {
      static int Count(ReadOnlySpan<char> span, char find)
      {
        var i = 0;
        foreach (var c in span)
          if (c.Equals(find))
            ++i;

        return i;
      }

      var thisType = source.FullName;
      var nextType = next.FullName;

      var name = thisType.AsSpan(nextType.Length);

      return IsJagged
        ? Count(name, '[')
        : Count(name, ',') + 1;
    }

    #endregion
  }
}
