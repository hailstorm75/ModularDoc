using MarkDoc.Members.Enums;
using System;

namespace MarkDoc.Members.Dnlib
{
  public class MethodDef
    : ContructorDef, IMethod
  {
    #region Properties
    
    /// <inheritdoc />
    public MemberVisibility Visibility { get; }

    /// <inheritdoc />
    public bool IsAsync { get; }

    /// <inheritdoc />
    public Lazy<IType?> Returns { get; }

    #endregion

    /// <summary>
    /// Default constructor
    /// </summary>
    public MethodDef()
      : base()
    {

    }
  }
}
