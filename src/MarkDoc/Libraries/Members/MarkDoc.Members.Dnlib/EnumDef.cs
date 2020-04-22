using System.Collections.Generic;

namespace MarkDoc.Members.Dnlib
{
  public class EnumDef
    : TypeDef, IEnum
  {
    #region Properties

    /// <inheritdoc />
    public IReadOnlyCollection<string> Fields { get; }

    #endregion

    /// <summary>
    /// Default constructor
    /// </summary>
    public EnumDef()
      : base()
    {

    }
  }
}
