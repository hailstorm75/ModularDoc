using System.Collections.Generic;

namespace MarkDoc.Documentation.Tags
{
  /// <summary>
  /// Interface for documentation list tags
  /// </summary>
  public interface IListTag
    : IContent
  {
    /// <summary>
    /// Type of given list
    /// </summary>
    public enum ListType
    {
      /// <summary>
      /// Numbered list
      /// </summary>
      Number,
      /// <summary>
      /// Bullet list
      /// </summary>
      Bullet,
      /// <summary>
      /// Table list
      /// </summary>
      Table,
    }

    #region Properties

    /// <summary>
    /// Type of list
    /// </summary>
    ListType Type { get; }
    /// <summary>
    /// List headings
    /// </summary>
    /// <remarks>
    /// Used when the list is a <see cref="ListType.Table"/>
    /// </remarks>
    IReadOnlyCollection<IContent> Headings { get; }
    /// <summary>
    /// List rows
    /// </summary>
    IReadOnlyCollection<IReadOnlyCollection<IContent>> Rows { get; }

    #endregion
  }
}
