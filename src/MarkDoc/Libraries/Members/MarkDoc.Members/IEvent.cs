using System;

namespace MarkDoc.Members
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
    Lazy<IType> Type { get; }
  }
}
