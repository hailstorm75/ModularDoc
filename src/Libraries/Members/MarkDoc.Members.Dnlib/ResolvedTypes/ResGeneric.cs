using System;
using System.Collections.Generic;
using System.Linq;
using dnlib.DotNet;
using MarkDoc.Helpers;
using MarkDoc.Members.Dnlib.Properties;
using MarkDoc.Members.ResolvedTypes;

namespace MarkDoc.Members.Dnlib.ResolvedTypes
{
  public class ResGeneric
    : ResType, IResGeneric
  {
    #region Properties

    public IReadOnlyCollection<IResType> Generics { get; } 

    #endregion

    internal ResGeneric(IResolver resolver, TypeSig source, IReadOnlyDictionary<string, string>? generics)
      : base(resolver, source, ResolveName(source), ResolveRawName(resolver, source, generics), source.FullName)
    {
      if (!(source is GenericInstSig token))
        throw new NotSupportedException(Resources.notGeneric);

      Generics = token.GenericArguments.Select(x => Resolver.Resolve(x, generics)).ToReadOnlyCollection();
    }

    private static string ResolveRawName(IResolver resolver, TypeSig source, IReadOnlyDictionary<string, string>? generics)
    {
      static string ResolveGenerics(string type, IReadOnlyDictionary<string, string>? generics)
      {
        if (generics != null && generics.TryGetValue(type, out var generic))
          return generic;
        return type;
      }

      if (!(source is GenericInstSig token))
        throw new NotSupportedException(Resources.notGeneric);

      var index = source.FullName.IndexOf('`', StringComparison.InvariantCultureIgnoreCase);
      var name = source.FullName.Remove(index);

      var result = $"{name}{{{string.Join(",",token.GenericArguments.Select(x => ResolveGenerics(resolver.Resolve(x, generics).DocumentationName, generics)))}}}";
      return result;
    }
  }
}
