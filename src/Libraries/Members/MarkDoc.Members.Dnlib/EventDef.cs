using dnlib.DotNet;
using MarkDoc.Members.Enums;
using System;
using System.Diagnostics;
using System.Threading;

namespace MarkDoc.Members.Dnlib
{
  [DebuggerDisplay(nameof(EventDef) + ": {Name}")]
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
    /// <param name="source">Member source</param>
    internal EventDef(IResolver resolver, dnlib.DotNet.EventDef source)
      : base(resolver, source)
    {
      Name = source.Name.String;
      Type = ResolveType(source);
    }

    private IResType ResolveType(dnlib.DotNet.EventDef source)
      => Resolver.Resolve(source.EventType.ToTypeSig());
  }
}
