using System;
using System.Linq;

namespace MarkDoc.Members.Dnlib
{
  public class ResArray
    : IResArray, IResType
  {
    #region Properties

    public IResType ArrayType { get; }

    public bool IsJagged { get; }

    public int Dimension { get; }

    public string DisplayName
      => ArrayType.DisplayName;

    public string Name
      => ArrayType.Name;

    public string TypeNamespace
      => ArrayType.TypeNamespace;

    #endregion

    public ResArray(dnlib.DotNet.TypeSig source)
    {
      if (source == null)
        throw new ArgumentNullException(nameof(source));

      IsJagged = source.ElementType == dnlib.DotNet.ElementType.SZArray;

      var next = ResolveNext(source, IsJagged);
      ArrayType = Resolver.Instance.Resolve(next);
      Dimension = ResolveDimension(source, next);
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
