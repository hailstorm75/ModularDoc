using MarkDoc.Members.Enums;
using System;

namespace MarkDoc.Members.Dnlib
{
  public class MethodDef
    : ConstructorDef, IMethod
  {
    #region Properties

    /// <inheritdoc />
    public MemberInheritance Inheritance { get; }

    /// <inheritdoc />
    public bool IsAsync { get; }

    /// <inheritdoc />
    public Lazy<IType?> Returns { get; }

    #endregion

    /// <summary>
    /// Default constructor
    /// </summary>
    public MethodDef(dnlib.DotNet.MethodDef source)
      : base(source)
    {
      if (source == null)
        throw new ArgumentNullException(nameof(source));

      Inheritance = ResolveInheritance(source);
    }

    private static MemberInheritance ResolveInheritance(dnlib.DotNet.MethodDef source)
    {
      if (source.IsVirtual && (source.Attributes & dnlib.DotNet.MethodAttributes.NewSlot) == 0)
        return MemberInheritance.Override;
      else if (source.IsVirtual)
        return MemberInheritance.Virtual;
      else if (source.IsAbstract)
        return MemberInheritance.Abstract;

      return MemberInheritance.Normal;
    }
  }
}
