using System.Collections.Generic;

namespace MarkDoc.Documentation.Tags
{
  /// <summary>
  /// Interface for documentation tags
  /// </summary>
  public interface ITag
  {
    /// <summary>
    /// Tag types
    /// </summary>
    public enum TagType
    {
      /// <summary>
      /// Invalid
      /// </summary>
      /// <remarks>
      /// These tags will be excluded
      /// </remarks>
      InvalidTag,
      /// <summary>
      /// Main description
      /// </summary>
      Summary,
      /// <summary>
      /// Remarks
      /// </summary>
      Remarks,
      /// <summary>
      /// Examples
      /// </summary>
      Example,
      /// <summary>
      /// Return description
      /// </summary>
      Returns,
      /// <summary>
      /// Property type description
      /// </summary>
      Value,
      /// <summary>
      /// Thrown exception
      /// </summary>
      /// <remarks>
      /// Has a cref (<see cref="Reference"/>)
      /// </remarks>
      Exception,
      /// <summary>
      /// Member parameter description
      /// </summary>
      /// <remarks>
      /// Has a name (<see cref="Reference"/>)
      /// </remarks>
      Param,
      /// <summary>
      /// Element or member generic type description
      /// </summary>
      /// <remarks>
      /// Has a name (<see cref="Reference"/>)
      /// </remarks>
      Typeparam,
      /// <summary>
      /// Reference to additional elements or members
      /// </summary>
      /// <remarks>
      /// Has a cref (<see cref="Reference"/>)
      /// </remarks>
      Seealso,
      /// <summary>
      /// Inherit doc
      /// </summary>
      /// <remarks>
      /// Documentation will be copied from derived elements
      /// <para/>
      /// Can have a cref (<see cref="Reference"/>)
      /// </remarks>
      Inheritdoc,
    }

    #region Properties

    /// <summary>
    /// Tag type
    /// </summary>
    /// <seealso cref="TagType"/>
    TagType Type { get; }

    /// <summary>
    /// Tag reference
    /// </summary>
    /// <remarks>
    /// Either holds cref or name
    /// </remarks>
    string Reference { get; }

    /// <summary>
    /// Tag inner content
    /// </summary>
    IReadOnlyCollection<IContent> Content { get; }

    #endregion
  }
}
