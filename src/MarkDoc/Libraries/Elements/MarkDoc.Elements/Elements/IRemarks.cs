using MarkDoc.Elements.Elements;

namespace MarkDoc.Elements
{
  /// <summary>
  /// Interface for remark elements
  /// </summary>
  public interface IRemarks
    : IElement
  {
    /// <summary>
    /// Element content
    /// </summary>
    IElement Content { get; }
  }
}
