namespace MarkDoc.Members.Dnlib
{
  public class ResValueType
    : ResType
  {
    internal ResValueType(IResolver resolver, dnlib.DotNet.TypeSig source, string displayName)
      : this(resolver, source, displayName, source.FullName)
    {
    }

    internal protected ResValueType(IResolver resolver, dnlib.DotNet.TypeSig source, string displayName, string rawName)
      : base(resolver, source, displayName, rawName)
    {
    }
  }
}
