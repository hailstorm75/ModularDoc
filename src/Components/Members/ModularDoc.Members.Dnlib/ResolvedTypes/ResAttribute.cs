using dnlib.DotNet;
using ModularDoc.Members.ResolvedTypes;

namespace ModularDoc.Members.Dnlib.ResolvedTypes;

public class ResAttribute
  : ResType, IResAttribute
{
  /// <summary>
  /// Default constructor
  /// </summary>
  public ResAttribute(Resolver resolver, TypeSig source)
    : base(resolver, source, ResolveName(source), ResolveDocName(source), source.FullName, false)
  {
  }
}