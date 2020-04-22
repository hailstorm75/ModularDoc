using System;
using System.Collections.Generic;
using System.Linq;

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
    public EnumDef(dnlib.DotNet.TypeDef source)
      : base(source)
    {
      if (source == null)
        throw new ArgumentNullException(nameof(source));

      Fields = source.Fields.Select(x => x.Name.String).ToArray();
    }
  }
}
