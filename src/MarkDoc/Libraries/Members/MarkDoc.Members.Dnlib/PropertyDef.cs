using MarkDoc.Members.Enums;
using System;

namespace MarkDoc.Members.Dnlib
{
  public class PropertyDef
    : MemberDef, IProperty
  {
    #region Properties

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

    }
  }
}
