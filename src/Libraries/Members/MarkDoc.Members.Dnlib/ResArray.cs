using System;
using System.Linq;
using System.Threading;

namespace MarkDoc.Members.Dnlib
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
    public string Name
    {
      get
      {
        if (IsJagged)
          return ArrayType.Name + string.Join(string.Empty, Enumerable.Repeat("[]", Dimension));
        return ArrayType.Name + $"[{string.Concat(Enumerable.Repeat(",", Dimension - 1))}]";
      }
    }

    /// <inheritdoc />
    public string TypeNamespace
      => ArrayType.TypeNamespace;

    /// <inheritdoc />
    public Lazy<IType?> Reference { get; }

    #endregion

    internal ResArray(IResolver resolver, dnlib.DotNet.TypeSig source)
    {
      if (source == null)
        throw new ArgumentNullException(nameof(source));
      if (resolver == null)
        throw new ArgumentNullException(nameof(resolver));

      IsJagged = source.ElementType == dnlib.DotNet.ElementType.SZArray;
      Resolver = resolver;

      var next = ResolveNext(source, IsJagged);
      ArrayType = Resolver.Resolve(next);
      Dimension = ResolveDimension(source, next);
      Reference = new Lazy<IType?>(() => Resolver.FindReference(source, this), LazyThreadSafetyMode.ExecutionAndPublication);
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
      var thisType = source.FullName;
      var nextType = next.FullName;

      var name = thisType.Substring(nextType.Length);
      if (IsJagged)
        return name.Count(x => x == '[');
      else
        return name.Count(x => x == ',') + 1;
    }

    #endregion
  }
}
