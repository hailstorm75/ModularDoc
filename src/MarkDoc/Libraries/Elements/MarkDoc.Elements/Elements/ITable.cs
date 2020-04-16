using MarkDoc.Elements.Elements;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MarkDoc.Elements
{
  /// <summary>
  /// Interface for the table element
  /// </summary>
  public interface ITable
    : IElement
  {
    #region Properties

    /// <summary>
    /// Table headings
    /// </summary>
    IReadOnlyCollection<string> Headings { get; }

    /// <summary>
    /// Table rows
    /// </summary>
    IList<string[]> Rows { get; } 

    #endregion

    /// <summary>
    /// Adds set of new <paramref name="rows"/>
    /// </summary>
    /// <param name="rows">Items to add</param>
    void AddRow(IReadOnlyCollection<string> rows)
    {
      if (rows == null)
        throw new ArgumentNullException(nameof(rows));
      if (rows.Count > Headings.Count)
        throw new ArgumentException($"Number of items in {nameof(rows)} is greater than {Headings.Count} count.");

      Rows.Add(rows.ToArray());
    }
  }
}
