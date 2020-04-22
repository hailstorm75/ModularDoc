namespace MarkDoc.Elements
{
  /// <summary>
  /// Interface for list elements
  /// </summary>
  public interface IList
    : IElement
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

    #region Methods

    /// <summary>
    /// Add <paramref name="text"/> to list
    /// </summary>
    /// <param name="text">Text to add</param>
    void Add(IText text);

    /// <summary>
    /// Add a sub-<paramref name="list"/>
    /// </summary>
    /// <param name="list">Sub-list to add</param>
    void Add(IList list); 

    #endregion
  }
}
