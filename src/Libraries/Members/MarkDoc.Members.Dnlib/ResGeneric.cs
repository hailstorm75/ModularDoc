using dnlib.DotNet;
using MarkDoc.Helpers;
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

    internal ResGeneric(IResolver resolver, dnlib.DotNet.TypeSig source)
      : base(resolver, source)
    {
      if (!(source is GenericInstSig token))
        throw new NotSupportedException(Resources.notGeneric);

      Generics = token.GenericArguments.Select(Resolver.Resolve).ToReadOnlyCollection();
    }
  }
}
