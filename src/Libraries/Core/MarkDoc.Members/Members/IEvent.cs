using MarkDoc.Members.Enums;
using MarkDoc.Members.ResolvedTypes;

namespace MarkDoc.Members.Members
{
  /// <summary>
  /// Interface for events
  /// </summary>
  public interface IEvent
    : IMember
  {
    /// <summary>
    /// Event inheritance type
    /// </summary>
    MemberInheritance Inheritance { get; }

    /// <summary>
    /// Event type
    /// </summary>
    IResType Type { get; }
  }
}
