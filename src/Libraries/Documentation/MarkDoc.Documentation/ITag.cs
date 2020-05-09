using System.Collections.Generic;
using MarkDoc.Documentation.Tags;

namespace MarkDoc.Documentation
{
  /// <summary>
  /// Interface for documentation tags
  /// </summary>
  public interface ITag
  {
    public enum TagType
    {
      InvalidTag,
      Summary,
      Remarks,
      Example,
      Returns,
      Value,
      Exception,
      Param,
      Typeparam,
      Seealso,
      Inheritdoc,
    }

    #region Properties

    TagType Type { get; }

    string Reference { get; }

    IReadOnlyCollection<IContent> Content { get; }

    #endregion
  }
}
