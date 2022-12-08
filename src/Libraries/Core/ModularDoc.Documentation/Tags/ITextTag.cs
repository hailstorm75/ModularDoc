namespace ModularDoc.Documentation.Tags
{
  /// <summary>
  /// Interface for tags containing text
  /// </summary>
  public interface ITextTag
    : IContent
  {
    /// <summary>
    /// Text content
    /// </summary>
    string Content { get; }
  }
}
