namespace MarkDoc.Elements
{
  /// <summary>
  /// Interface for teext elements
  /// </summary>
  public interface IText
    : IElement
  {
    /// <summary>
    /// Text content
    /// </summary>
    string Content { get; }
  }
}
