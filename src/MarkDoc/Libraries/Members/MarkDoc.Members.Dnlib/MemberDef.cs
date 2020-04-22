using MarkDoc.Members.Enums;

namespace MarkDoc.Members.Dnlib
{
  public abstract class MemberDef
    : IMember
  {
    #region Properties

    /// <inheritdoc />
    public bool IsObsolete { get; }

    /// <inheritdoc />
    public bool IsStatic { get; }

    /// <inheritdoc />
    public string Name { get; }

    /// <inheritdoc />
    public AccessorType Accessor { get; }

    #endregion

    /// <summary>
    /// Default constructor
    /// </summary>
    protected MemberDef()
    {

    }
  }
}
