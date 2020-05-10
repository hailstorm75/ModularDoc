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

    internal ResGeneric(IResolver resolver, TypeSig source, IReadOnlyDictionary<string, string>? generics)
      : base(resolver, source, ResolveName(source), ResolveRawName(source, generics))
    {
      if (!(source is GenericInstSig token))
        throw new NotSupportedException(Resources.notGeneric);

      Generics = token.GenericArguments.Select(x => Resolver.Resolve(x, default)).ToReadOnlyCollection();
    }

    private static string ResolveRawName(TypeSig source, IReadOnlyDictionary<string, string>? generics)
    {
      string ResolveGenerics(string type)
      {
        if (generics != null && generics.TryGetValue(type, out var generic))
          return generic;
        return type;
      }

      if (!(source is GenericInstSig token))
        throw new NotSupportedException(Resources.notGeneric);

      var index = source.FullName.IndexOf('`', StringComparison.InvariantCultureIgnoreCase);
      var name = source.FullName.Remove(index);

      var result = $"{name}{{{string.Join(",",token.GenericArguments.Select(x => ResolveGenerics(x.TypeName)))}}}";
      return result;
    }
  }
}
