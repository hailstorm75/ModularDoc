namespace MarkDoc.Members.Dnlib
{
  public class ResValueType
    : ResType
  {
    internal ResValueType(IResolver resolver, dnlib.DotNet.TypeSig source, string displayName)
      : base(resolver, source, displayName)
    {
    }
  }
}
