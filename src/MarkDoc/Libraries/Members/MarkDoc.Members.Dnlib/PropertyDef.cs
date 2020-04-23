using MarkDoc.Members.Enums;
using System;
using System.Threading;
using System.Diagnostics;
using System.Linq;

namespace MarkDoc.Members.Dnlib
{
  [DebuggerDisplay(nameof(PropertyDef) + ": {Name}")]
  public class PropertyDef
    : MemberDef, IProperty
  {
    #region Properties

    /// <inheritdoc />
    public override string Name { get; }

    /// <inheritdoc />
    public override bool IsStatic { get; }

    /// <inheritdoc />
    public MemberInheritance Visibility { get; }

    /// <inheritdoc />
    public Lazy<IType> Type { get; }

    /// <inheritdoc />
    public AccessorType GetAccessor { get; }

    /// <inheritdoc />
    public AccessorType SetAccessor { get; }

    #endregion

    /// <summary>
    /// Default constructor
    /// </summary>
    public PropertyDef(dnlib.DotNet.PropertyDef source)
      : base(source)
    {
      Name = source.Name;
      IsStatic = source.SetMethods.Concat(source.GetMethods).First().IsStatic;
      Type = new Lazy<IType>(() => default, LazyThreadSafetyMode.ExecutionAndPublication);
      GetAccessor = ResolveAccessor(source.GetMethod);
      SetAccessor = ResolveAccessor(source.SetMethod);
    }

    private static AccessorType ResolveAccessor(dnlib.DotNet.MethodDef method)
    {
      if (method.Access == dnlib.DotNet.MethodAttributes.Public)
        return AccessorType.Public;

      return AccessorType.Protected; // TODO: Add missing condtions
    }
  }
}
