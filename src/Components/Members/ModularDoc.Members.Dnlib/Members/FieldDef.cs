using ModularDoc.Members.Dnlib.Properties;
using ModularDoc.Members.Enums;
using ModularDoc.Members.Members;
using System;

namespace ModularDoc.Members.Dnlib.Members
{
  public class FieldDef
    : MemberDef, IField
  {
    #region Properties

    public override bool IsStatic { get; }

    public override string Name { get; }

    public override string RawName { get; }

    public override AccessorType Accessor { get; }

    #endregion

    public FieldDef(Resolver resolver, dnlib.DotNet.FieldDef source, bool isNested)
      : base(resolver, source)
    {
      IsStatic = source.IsStatic;
      Name = source.Name;
      RawName = source.FullName;
      Accessor = ResolveAccessor(source);
    }

    private static AccessorType ResolveAccessor(dnlib.DotNet.FieldDef field)
      => field.Access switch
      {
        dnlib.DotNet.FieldAttributes.Public => AccessorType.Public,
        dnlib.DotNet.FieldAttributes.Private => AccessorType.Private,
        dnlib.DotNet.FieldAttributes.Family => AccessorType.Protected,
        dnlib.DotNet.FieldAttributes.Assembly => AccessorType.Internal,
        dnlib.DotNet.FieldAttributes.FamORAssem => AccessorType.ProtectedInternal,
        _ => throw new NotSupportedException(Resources.accessorPrivate)
      };
  }
}
