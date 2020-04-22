using MarkDoc.Elements.Members;
using MarkDoc.Elements.Members.Enums;
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
    /// Argument keyword
    /// </summary>
    ArgumentType Keyword { get; } 

    /// <summary>
    /// Argument type
    /// </summary>
    Lazy<IType> Type { get; }
  }
}
