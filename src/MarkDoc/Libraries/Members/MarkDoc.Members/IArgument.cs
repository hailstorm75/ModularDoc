using MarkDoc.Members.Enums;
using System;

namespace MarkDoc.Members
{
  /// <summary>
  /// Interface for <see cref="IMethod"/> arguments
  /// </summary>
  public interface IArgument
    : IMember
  {
    /// <summary>
    /// Argument keyword
    /// </summary>
    ArgumentType Keyword { get; } 

    /// <summary>
    /// Argument type
    /// </summary>
    Lazy<IType> Type { get; }
  }
}
