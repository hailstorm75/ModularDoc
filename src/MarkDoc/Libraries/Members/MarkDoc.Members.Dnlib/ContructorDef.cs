using System.Collections.Generic;

namespace MarkDoc.Members.Dnlib
{
  public class ContructorDef
    : MemberDef, IConstructor
  {
    #region Properties

    /// <inheritdoc />
    public IReadOnlyCollection<IArgument> Arguments { get; }

    #endregion

    /// <summary>
    /// Default constructor
    /// </summary>
    public ContructorDef()
      : base()
    {

    }
  }
}