using MarkDoc.Elements.Members;
using System;

namespace MarkDoc.Elements
{
  /// <summary>
  /// Interface for <see cref="IMethod"/> arguments
  /// </summary>
  public interface IArgument
    : IMember
  {
    /// <summary>
    /// Types of arguments
    /// </summary>
    public enum ArgumentType
    {
      /// <summary>
      /// Argument with no keyword
      /// </summary>
      Normal,
      /// <summary>
      /// Argument with the ref keyword
      /// </summary>
      Ref,
      /// <summary>
      /// Argument with the out keyword
      /// </summary>
      Out
    }

    #region Properties

    /// <summary>
    /// Argument keyword
    /// </summary>
    ArgumentType Keyword { get; } 

    /// <summary>
    /// Argument type
    /// </summary>
    Lazy<IType> Type { get; }

    #endregion
  }
}
