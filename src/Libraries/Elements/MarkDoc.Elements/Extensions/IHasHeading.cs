namespace MarkDoc.Elements.Extensions
{
  /// <summary>
  /// Interface for types which have headings
  /// </summary>
  public interface IHasHeading
  {
    /// <summary>
    /// Heading text
    /// </summary>
    string Heading { get; set; }
    /// <summary>
    /// Heading level
    /// </summary>
    int Level { get; set; }
  }
}
