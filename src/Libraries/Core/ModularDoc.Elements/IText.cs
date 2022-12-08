using ModularDoc.Elements.Extensions;

namespace ModularDoc.Elements
{
  /// <summary>
  /// Interface for text elements
  /// </summary>
  public interface IText
    : ITextContent, IHasContent<string>
  {
    /// <summary>
    /// Text styles
    /// </summary>
    public enum TextStyle
    {
      /// <summary>
      /// No style
      /// </summary>
      Normal,
      /// <summary>
      /// Italic style
      /// </summary>
      Italic,
      /// <summary>
      /// Bold style
      /// </summary>
      Bold,
      /// <summary>
      /// Single line code style
      /// </summary>
      CodeInline,
      /// <summary>
      /// Multi-line code style
      /// </summary>
      Code
    }

    /// <summary>
    /// Text element style
    /// </summary>
    /// <seealso cref="TextStyle"/>
    TextStyle Style { get; }
  }
}
