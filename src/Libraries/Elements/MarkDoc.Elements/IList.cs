using MarkDoc.Elements.Extensions;
using System.Collections.Generic;

namespace MarkDoc.Elements
{
  /// <summary>
  /// Interface for list elements
  /// </summary>
  public interface IList
    : IElement, IHasContent<IReadOnlyCollection<IElement>>, IHasHeading
  {
    /// <summary>
    /// Possilbe list types
    /// </summary>
    public enum ListType
    {
      /// <summary>
      /// List items will be numbered
      /// </summary>
      Numbered,
      /// <summary>
      /// List items will be dotted
      /// </summary>
      Dotted
    }

    /// <summary>
    /// List type
    /// </summary>
    ListType Type { get; }

    /// <summary>
    /// Prints element to a string
    /// </summary>
    /// <param name="indent">List indentation level</param>
    /// <returns>Converted list</returns>
    string ToString(int indent);
  }
}
