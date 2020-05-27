using System;

namespace MarkDoc.Members.Dnlib.ResolvedTypes
{
  public class ResValueType
    : ResType
  {
    internal ResValueType(IResolver resolver, dnlib.DotNet.TypeSig source, string displayName)
      : this(resolver, source, displayName, source.FullName)
    {
    }

    protected ResValueType(IResolver resolver, dnlib.DotNet.TypeSig source, string displayName, string docName)
      : base(resolver, source, displayName, docName, source?.FullName ?? throw new ArgumentNullException(nameof(source)))
    {
    }
  }
}
