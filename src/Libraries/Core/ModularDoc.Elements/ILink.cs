using System;
using ModularDoc.Elements.Extensions;

namespace ModularDoc.Elements
{
  /// <summary>
  /// Interface for link elements
  /// </summary>
  public interface ILink
    : ITextContent, IHasContent<IText>
  {
    /// <summary>
    /// Link reference
    /// </summary>
    /// <value>String containing a URI</value>
    Lazy<string> Reference { get; }
  }
}
