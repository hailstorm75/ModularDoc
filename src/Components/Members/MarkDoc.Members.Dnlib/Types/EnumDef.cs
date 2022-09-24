using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using MarkDoc.Helpers;
using MarkDoc.Members.Dnlib.Members;
using MarkDoc.Members.Enums;
using MarkDoc.Members.Members;
using MarkDoc.Members.Types;

namespace MarkDoc.Members.Dnlib.Types
{
  /// <summary>
  /// Class for representing enums
  /// </summary>
  [DebuggerDisplay(nameof(EnumDef) + (": {" + nameof(Name) + "}"))]
  public class EnumDef
    : TypeDef, IEnum
  {
    #region Properties

    /// <inheritdoc />
    public IReadOnlyCollection<IEnumField> Fields { get; }

    #endregion

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="resolver">Type resolver instance</param>
    /// <param name="source">Type source</param>
    /// <param name="parent">Nested type parent</param>
    internal EnumDef(Resolver resolver, dnlib.DotNet.TypeDef source, dnlib.DotNet.TypeDef? parent, DotNetType type)
      : base(resolver, source, parent, type)
    {
      // If the source is null..
      if (source is null)
        // throw an exception
        throw new ArgumentNullException(nameof(source));

      // Initialize the enum fields
      Fields = source.Fields
        // Select valid enum fields
        .Where(x => x.ElementType != dnlib.DotNet.ElementType.End)
        // Initialize the enum fields
        .Select(x => new EnumFieldDef(x, Accessor))
        // Materialize the collection
        .ToReadOnlyCollection();
    }
  }
}
