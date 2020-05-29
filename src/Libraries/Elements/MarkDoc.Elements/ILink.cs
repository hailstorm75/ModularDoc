using MarkDoc.Elements.Extensions;

namespace MarkDoc.Elements
{
  /// <summary>
  /// Interface for link elements
  /// </summary>
  public interface ILink
    : IElement, IHasContent<IText>
  {
    /// <summary>
    /// Link reference
    /// </summary>
    /// <value>String containing a URI</value>
    string Reference { get; }
  }
}
