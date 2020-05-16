using MarkDoc.Elements.Extensions;

namespace MarkDoc.Elements
{
  /// <summary>
  /// Interface for teext elements
  /// </summary>
  public interface IText
    : IElement, IHasContent<string>
  {
    public enum TextStyle
    {
      Normal,
      Italic,
      Bold,
      CodeInline,
      Code
    }

    TextStyle Style { get; set; }
  }
}
