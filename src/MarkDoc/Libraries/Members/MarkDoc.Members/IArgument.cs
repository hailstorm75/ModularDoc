using MarkDoc.Members.Enums;
using System;

namespace MarkDoc.Members
{
  /// <summary>
  /// Interface for <see cref="IMethod"/> arguments
  /// </summary>
  public interface IArgument
  {
    /// <summary>
    /// Argument name
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Argument keyword
    /// </summary>
    ArgumentType Keyword { get; } 

    /// <summary>
    /// Argument type
    /// </summary>
    IResType Type { get; }
  }
}
