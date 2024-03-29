﻿using System;
using System.Diagnostics;
using dnlib.DotNet;
using ModularDoc.Members.Enums;
using ModularDoc.Members.Members;
using ModularDoc.Members.ResolvedTypes;

namespace ModularDoc.Members.Dnlib.Members
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
    public MemberInheritance Inheritance { get; }

    /// <inheritdoc />
    public IResType Type { get; }

    public override string RawName { get; }

    #endregion

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="resolver">Type resolver instance</param>
    /// <param name="source">Member source</param>
    internal EventDef(Resolver resolver, dnlib.DotNet.EventDef source)
      : base(resolver, source)
    {
      Name = source.Name.String;
      Type = ResolveType(source);
      IsStatic = source.AddMethod.IsStatic;
      Inheritance = ResolveInheritance(source.RemoveMethod);
      Accessor = ResolveAccessor(source.AddMethod);
      RawName = ResolveRawName(source);
    }

    #region Methods

    private static string ResolveRawName(dnlib.DotNet.EventDef source)
    {
      var rawName = source.FullName.AsSpan(source.FullName.IndexOf(' ', StringComparison.InvariantCultureIgnoreCase) + 1);

      return rawName.ToString()
        .Replace("::", ".", StringComparison.InvariantCultureIgnoreCase)
        .Replace("/", ".", StringComparison.InvariantCultureIgnoreCase);
    }

    private static MemberInheritance ResolveInheritance(dnlib.DotNet.MethodDef source)
    {
      if (source.IsVirtual && (source.Attributes & MethodAttributes.NewSlot) == 0)
        return MemberInheritance.Override;
      if (source.IsAbstract)
        return MemberInheritance.Abstract;
      if (source.IsVirtual)
        return MemberInheritance.Virtual;

      return MemberInheritance.Normal;
    }

    private IResType ResolveType(dnlib.DotNet.EventDef source)
      => Resolver.Resolve(source.EventType.ToTypeSig());

    private static AccessorType ResolveAccessor(dnlib.DotNet.MethodDef method)
      => method.Access switch
      {
        MethodAttributes.Private => AccessorType.Private,
        MethodAttributes.Public => AccessorType.Public,
        MethodAttributes.Family => AccessorType.Protected,
        MethodAttributes.FamORAssem => AccessorType.ProtectedInternal,
        _ => AccessorType.Internal
      };

    #endregion
  }
}
