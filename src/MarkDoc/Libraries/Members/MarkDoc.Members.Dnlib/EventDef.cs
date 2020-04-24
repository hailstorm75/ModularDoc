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
    public Lazy<IType> Type { get; }

    #endregion

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="source">Member source</param>
    internal EventDef(dnlib.DotNet.EventDef source)
      : base(source)
    {
      // TODO: Implement the rest
      Name = source.Name.String;
      Type = new Lazy<IType>(() => default, LazyThreadSafetyMode.ExecutionAndPublication);
    }
  }
}
