namespace MarkDoc.Elements
{
  /// <summary>
  /// Interface for <see cref="IMethod"/> arguments
  /// </summary>
  public interface IArgument
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
    /// Argument name
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Argument type
    /// </summary>
    ArgumentType Type { get; } 

    #endregion
  }
}
