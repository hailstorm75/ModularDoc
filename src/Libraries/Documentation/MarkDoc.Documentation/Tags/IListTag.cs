using System.Collections.Generic;

namespace MarkDoc.Documentation.Tags
{
  public interface IListTag
    : IContent
  {
    public enum ListType
    {
      Number,
      Bullet,
      Table,
    }

    #region Properties

    ListType Type { get; }
    IReadOnlyCollection<IContent> Headings { get; }
    IReadOnlyCollection<IReadOnlyCollection<IContent>> Rows { get; } 

    #endregion
  }
}
