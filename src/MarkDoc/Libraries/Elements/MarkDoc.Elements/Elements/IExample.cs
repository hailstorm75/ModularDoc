using MarkDoc.Elements.Elements;

namespace MarkDoc.Elements
{
  /// <summary>
  /// Interface for example elements
  /// </summary>
  public interface IExample
    : IElement
  {
    /// <summary>
    /// Element content
    /// </summary>
    IElement Content { get; }
  }
}
