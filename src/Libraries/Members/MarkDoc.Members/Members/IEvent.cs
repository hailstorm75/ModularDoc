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
    /// Event type
    /// </summary>
    IResType Type { get; }
  }
}
