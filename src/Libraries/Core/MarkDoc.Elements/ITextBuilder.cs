using MarkDoc.Elements.Extensions;
using System.Collections.Generic;

namespace MarkDoc.Elements
{
  public interface ITextBuilder
    : ITextContent, IHasContent<IReadOnlyCollection<ITextContent>>
  {
    string Delimiter { get; }
  }
}
