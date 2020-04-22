namespace MarkDoc.Members.Dnlib
{
  public abstract class TypeDef
    : MemberDef, IType
  {
    #region Properties

    /// <inheritdoc />
    public string TypeNamespace { get; }

    #endregion

    /// <summary>
    /// Default constructor
    /// </summary>
    protected TypeDef()
      : base()
    {

    }
  }
}
