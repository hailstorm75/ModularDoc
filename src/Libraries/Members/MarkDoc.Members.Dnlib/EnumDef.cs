using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace MarkDoc.Members.Dnlib
{
  [DebuggerDisplay(nameof(EnumDef) + ": {Name}")]
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
    internal EnumDef(IResolver resolver, dnlib.DotNet.TypeDef source, dnlib.DotNet.TypeDef? parent)
      : base(resolver, source, parent)
    {
      if (source == null)
        throw new ArgumentNullException(nameof(source));

      Fields = source.Fields.Where(x => x.ElementType != dnlib.DotNet.ElementType.End)
                            .Select(x => x.Name.String).ToArray();
    }
  }
}
