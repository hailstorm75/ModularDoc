using System;
using System.Diagnostics;
using dnlib.DotNet;
using ModularDoc.Members.Enums;
using ModularDoc.Members.Members;

namespace ModularDoc.Members.Dnlib.Members
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

    /// <inheritdoc />
    public (int line, string source)? LineSource => null;

    #endregion

    internal EnumFieldDef(IFullName source, AccessorType accessor)
    {
      // If the source is null..
      if (source is null)
        // throw an exception
        throw new ArgumentNullException(nameof(source));

      Name = source.Name.String;
      RawName = source.FullName.Substring(source.FullName.IndexOf(' ') + 1).Replace("::",".", StringComparison.InvariantCultureIgnoreCase).Replace("/", ".", StringComparison.InvariantCultureIgnoreCase);
      Accessor = accessor;
    }
  }
}