using System;
using System.Collections.Generic;

namespace MarkDoc.Documentation
{
  /// <summary>
  /// Interface for element documentation
  /// </summary>
  public interface IDocElement
  {
    /// <summary>
    /// Element name
    /// </summary>
    string Name { get; }
    /// <summary>
    /// Element documentation
    /// </summary>
    IDocumentation Documentation { get; }
    /// <summary>
    /// Element members
    /// </summary>
    Lazy<IReadOnlyDictionary<string, IDocMember>> Members { get; }
  }
}
