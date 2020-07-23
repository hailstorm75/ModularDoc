using System;
using dnlib.DotNet;
using MarkDoc.Members.Enums;
using MarkDoc.Members.Members;

namespace MarkDoc.Members.Dnlib.Members
{
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
      if (source is null)
        throw new ArgumentNullException(nameof(source));

      Name = source.Name.String;
      RawName = source.FullName;
      Accessor = accessor;
    }
  }
}