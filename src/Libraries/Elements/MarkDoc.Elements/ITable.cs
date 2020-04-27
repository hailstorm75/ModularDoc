using MarkDoc.Elements.Extensions;
using System.Collections.Generic;

namespace MarkDoc.Elements
{
  /// <summary>
  /// Interface for the table element
  /// </summary>
  public interface ITable
    : IElement, IHasHeading
  {
    #region Properties

    /// <summary>
    /// Table headings
    /// </summary>
    IReadOnlyCollection<string> Headings { get; }

    /// <summary>
    /// Table rows
    /// </summary>
    IReadOnlyCollection<IReadOnlyCollection<string>> Rows { get; } 

    #endregion
  }
}
