﻿namespace ModularDoc.Members.Enums
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
    Out,
    /// <summary>
    /// Argument with the in keyword
    /// </summary>
    In,
    /// <summary>
    /// Argument is optional
    /// </summary>
    Optional,
    /// <summary>
    /// Argument is variadic
    /// </summary>
    Param,
  }
}
