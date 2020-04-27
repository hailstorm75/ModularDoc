using dnlib.DotNet;
using MarkDoc.Members.Dnlib.Properties;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MarkDoc.Members.Dnlib
{
  public class ResGeneric
    : ResType, IResGeneric
  {
    #region Properties

    public IReadOnlyCollection<IResType> Generics { get; } 

    #endregion

    public ResGeneric(dnlib.DotNet.TypeSig source)
      : base(source)
    {
      if (!(source is GenericInstSig token))
        throw new NotSupportedException(Resources.notGeneric);

      Generics = token.GenericArguments.Select(Resolver.Instance.Resolve).ToArray();
    }
  }
}
