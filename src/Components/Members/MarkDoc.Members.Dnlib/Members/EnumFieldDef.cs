using System;
using System.Diagnostics;
using dnlib.DotNet;
using MarkDoc.Members.Enums;
using MarkDoc.Members.Members;

namespace MarkDoc.Members.Dnlib.Members
{
  /// <summary>
  /// Class for representing enum fields
  /// </summary>
  [DebuggerDisplay(nameof(EnumFieldDef) + (": {" + nameof(Name) + "}"))]
  public class EnumFieldDef
    : IEnumField
  {
    #region Properties

    public bool IsStatic => false;

    public string Name { get; }

    public string RawName { get; }

    public AccessorType Accessor { get; }

    #endregion

    internal EnumFieldDef(IFullName source, AccessorType accessor)
    {
      // If the source is null..
      if (source is null)
        // throw an exception
        throw new ArgumentNullException(nameof(source));

      Name = source.Name.String;
      RawName = source.FullName.Replace("::",".", StringComparison.InvariantCultureIgnoreCase).Replace("/", ".", StringComparison.InvariantCultureIgnoreCase);
      Accessor = accessor;
    }
  }
}