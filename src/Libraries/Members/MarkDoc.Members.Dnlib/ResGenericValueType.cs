using System;
using System.Collections.Generic;
using System.Text;

namespace MarkDoc.Members.Dnlib
{
  public class ResGenericValueType
    : ResValueType
  {
    public ResGenericValueType(IResolver resolver, dnlib.DotNet.TypeSig source, IReadOnlyDictionary<string, string>? generics)
      : base(resolver, source ?? throw new ArgumentNullException(nameof(source)), source.FullName, ResolveName(source, generics))
    {
    }

    private static string ResolveName(dnlib.DotNet.TypeSig source, IReadOnlyDictionary<string, string>? generics)
    {
      if (generics != null && generics.TryGetValue(source.FullName, out var result))
        return result;
      return source.FullName;
    }
  }
}
