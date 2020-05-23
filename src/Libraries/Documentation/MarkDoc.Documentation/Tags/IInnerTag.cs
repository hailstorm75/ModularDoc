using System.Collections.Generic;

namespace MarkDoc.Documentation.Tags
{
  public interface IInnerTag
    : IContent
  {
    public enum InnerTagType
    {
      ParamRef,
      TypeRef,
      Code,
      CodeSingle,
      See,
      SeeAlso,
      Para,
      InvalidTag,
    }

    #region Properties

    InnerTagType Type { get; }
    string Reference { get; }
    IReadOnlyCollection<IContent> Content { get; } 

    #endregion
  }
}
