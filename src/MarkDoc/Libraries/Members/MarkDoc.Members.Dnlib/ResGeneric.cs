using dnlib.DotNet;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace MarkDoc.Members.Dnlib
{
  public class ResGeneric
    : ResType, IResGeneric
  {
    public IReadOnlyCollection<IResType> Generics { get; }

    public ResGeneric(dnlib.DotNet.TypeSig source)
      : base(source)
    {
      if (!(source is GenericInstSig token))
        throw new NotSupportedException();

      Generics = token.GenericArguments.Select(Resolver.Instance.Resolve).ToArray();
    }
  }
}
