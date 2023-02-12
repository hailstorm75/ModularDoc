using System.Collections.Generic;

namespace ModularDoc.Documentation.Tags
{
  /// <summary>
  /// Interface for tags which are within other tags
  /// </summary>
  public interface IInnerTag
    : IContent
  {
    /// <summary>
    /// Tag type
    /// </summary>
    public enum InnerTagType
    {
      /// <summary>
      /// Parameter reference
      /// </summary>
      ParamRef,
      /// <summary>
      /// Type reference
      /// </summary>
      TypeRef,
      /// <summary>
      /// Code
      /// </summary>
      Code,
      /// <summary>
      /// Inline code
      /// </summary>
      CodeSingle,
      /// <summary>
      /// See reference
      /// </summary>
      See,
      /// <summary>
      /// See also reference
      /// </summary>
      SeeAlso,
      /// <summary>
      /// Paragraph
      /// </summary>
      Para,
      /// <summary>
      /// Invalid tag
      /// </summary>
      InvalidTag,
    }

    #region Properties

    /// <summary>
    /// Tag type
    /// </summary>
    InnerTagType Type { get; }

    /// <summary>
    /// Tag reference
    /// </summary>
    /// <remarks>
    /// Either holds cref or name
    /// </remarks>
    string Reference { get; }

    /// <summary>
    /// Hyperlink reference
    /// </summary>
    /// <remarks>
    /// Primarily for the See/SeeAlso tags. Holds value from href
    /// </remarks>
    string Link { get; }

    /// <summary>
    /// Tag content
    /// </summary>
    IReadOnlyCollection<IContent> Content { get; }

    #endregion
  }
}
