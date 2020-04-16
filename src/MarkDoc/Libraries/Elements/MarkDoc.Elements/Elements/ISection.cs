using MarkDoc.Elements.Elements;

namespace MarkDoc.Elements
{
  /// <summary>
  /// Interface for section elements
  /// </summary>
  public interface ISection
    : IElement
  {
    /// <summary>
    /// Adds <paramref name="element"/> content
    /// </summary>
    /// <param name="element">Element to add</param>
    void Add(IElement element);
  }
}
