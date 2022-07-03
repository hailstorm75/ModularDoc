using MarkDoc.Members.Enums;
using MarkDoc.Members.Types;

namespace MarkDoc.Members.Dnlib.Types
{
  /// <summary>
  /// Class for representing records
  /// </summary>
  public class RecordDef
    : ClassDef, IRecord
  {
    /// <inheritdoc />
    internal RecordDef(Resolver resolver, dnlib.DotNet.TypeDef source, dnlib.DotNet.TypeDef? parent, DotNetType type)
      : base(resolver, source, parent, type)
    {
    }
  }
}