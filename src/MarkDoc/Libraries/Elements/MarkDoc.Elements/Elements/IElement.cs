namespace MarkDoc.Elements.Elements
{
  /// <summary>
  /// Interface for elements
  /// </summary>
  public interface IElement
  {
    /// <summary>
    /// Sets the element <paramref name="heading"/> and its <paramref name="level"/>
    /// </summary>
    /// <param name="heading">Heading text</param>
    /// <param name="level">Heading level</param>
    void SetHeading(string heading, int level);

    /// <summary>
    /// Converts given element to a string
    /// </summary>
    /// <returns>Conversion result</returns>
    string ToString();
  }
}
