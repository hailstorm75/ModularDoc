using MarkDoc.Elements.Extensions;
using System.Collections.Generic;

namespace MarkDoc.Elements
{
  /// <summary>
  /// Interface for joining multiple <see cref="ITextContent"/> elements
  /// </summary>
  public interface ITextBuilder
    : ITextContent, IHasContent<IReadOnlyCollection<ITextContent>>
  {
    /// <summary>
    /// Joined text delimiter
    /// </summary>
    string Delimiter { get; }
  }
}
