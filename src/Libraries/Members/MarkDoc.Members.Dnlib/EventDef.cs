using dnlib.DotNet;
using MarkDoc.Members.Enums;
using System.Diagnostics;
using MarkDoc.Members.ResolvedTypes;

namespace MarkDoc.Members.Dnlib
{
  [DebuggerDisplay(nameof(EventDef) + (": {" + nameof(Name) + "}"))]
  public class EventDef
    : MemberDef, IEvent
  {
    #region Properties

    /// <inheritdoc />
    public override bool IsStatic { get; }

    /// <inheritdoc />
    public override string Name { get; }

    /// <inheritdoc />
    public override AccessorType Accessor { get; } 

    /// <inheritdoc />
    public IResType Type { get; }

    public override string RawName { get; } = string.Empty;

    #endregion

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="resolver">Type resolver instance</param>
    /// <param name="source">Member source</param>
    internal EventDef(IResolver resolver, dnlib.DotNet.EventDef source)
      : base(resolver, source)
    {
      // TODO: Assign accessor and static

      Name = source.Name.String;
      Type = ResolveType(source);
    }

    private IResType ResolveType(dnlib.DotNet.EventDef source)
      => Resolver.Resolve(source.EventType.ToTypeSig());
  }
}
