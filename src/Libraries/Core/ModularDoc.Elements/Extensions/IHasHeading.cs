namespace ModularDoc.Elements.Extensions
{
  /// <summary>
  /// Interface for types which have headings
  /// </summary>
  public interface IHasHeading
  {
    /// <summary>
    /// Heading text
    /// </summary>
    string Heading { get; }
    /// <summary>
    /// Heading level
    /// </summary>
    int Level { get; }
  }
}
