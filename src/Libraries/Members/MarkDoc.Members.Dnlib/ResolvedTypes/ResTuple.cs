using System;
using System.Collections.Generic;
using System.Linq;
using dnlib.DotNet;
using MarkDoc.Helpers;
using MarkDoc.Members.Dnlib.Properties;
using MarkDoc.Members.ResolvedTypes;

namespace MarkDoc.Members.Dnlib.ResolvedTypes
{
  public class ResTuple
    : ResType, IResTuple
  {
    #region Properties

    public IReadOnlyCollection<(string, IResType)> Fields { get; }

    public bool IsValueTuple { get; } 

    #endregion

    internal ResTuple(IResolver resolver, TypeSig source)
      : base(resolver, source)
    {
      if (!(source is GenericInstSig token))
        throw new NotSupportedException(Resources.notTuple);

      // TODO: Assign is value tuple

      Fields = token.GenericArguments.Select((x, i) => ($"Item{i + 1}", Resolver.Resolve(x))).ToReadOnlyCollection();
    }
  }
}
