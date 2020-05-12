namespace MarkDoc.Members.Dnlib.ResolvedTypes
{
  public class ResValueType
    : ResType
  {
    internal ResValueType(IResolver resolver, dnlib.DotNet.TypeSig source, string displayName)
      : this(resolver, source, displayName, source.FullName)
    {
    }

    protected ResValueType(IResolver resolver, dnlib.DotNet.TypeSig source, string displayName, string rawName)
      : base(resolver, source, displayName, rawName)
    {
    }
  }
}
