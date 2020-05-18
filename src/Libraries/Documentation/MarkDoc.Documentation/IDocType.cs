using System;
using System.Collections.Generic;

namespace MarkDoc.Documentation
{
  public interface IDocElement
  {
    string Name { get; }
    IDocumentation Documentation { get; }
    Lazy<IReadOnlyDictionary<string, IDocMember>> Members { get; }
  }
}
