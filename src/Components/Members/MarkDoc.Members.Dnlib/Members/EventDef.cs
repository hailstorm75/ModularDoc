using System.Diagnostics;
using dnlib.DotNet;
using MarkDoc.Members.Enums;
using MarkDoc.Members.Members;
using MarkDoc.Members.ResolvedTypes;

namespace MarkDoc.Members.Dnlib.Members
{
  /// <summary>
  /// Class for representing event members
  /// </summary>
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
      Name = source.Name.String;
      Type = ResolveType(source);
      IsStatic = source.AddMethod.IsStatic;
      Accessor = ResolveAccessor(source.AddMethod);
    }

    #region Methods

    private IResType ResolveType(dnlib.DotNet.EventDef source)
      => Resolver.Resolve(source.EventType.ToTypeSig());

    private static AccessorType ResolveAccessor(dnlib.DotNet.MethodDef method)
      => method.Access switch
      {
        MethodAttributes.Public => AccessorType.Public,
        MethodAttributes.Family => AccessorType.Protected,
        _ => AccessorType.Internal
      };

    #endregion
  }
}
