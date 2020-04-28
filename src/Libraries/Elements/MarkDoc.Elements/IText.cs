using MarkDoc.Elements.Extensions;

namespace MarkDoc.Elements
{
  /// <summary>
  /// Interface for teext elements
  /// </summary>
  public interface IText
    : IElement, IHasContent<string>
  {
  }
}
