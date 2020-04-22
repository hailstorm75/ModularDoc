using System.Collections.Generic;

namespace MarkDoc.Elements
{
  /// <summary>
  /// Interface for page elements
  /// </summary>
  public interface IPage
    : IElement
  {
    #region Properties

    /// <summary>
    /// Page content
    /// </summary>
    IList<IElement> Content { get; }

    #endregion

    /// <summary>
    /// Sets the <see cref="Content"/> of the page
    /// </summary>
    /// <param name="elements">Content to set</param>
    void SetContent(IReadOnlyCollection<IElement> elements);
  }
}
