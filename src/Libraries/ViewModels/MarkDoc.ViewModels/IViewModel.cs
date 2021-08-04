using System.Collections.Generic;

namespace MarkDoc.ViewModels
{
  public interface IViewModel
  {
    void SetArguments(IReadOnlyCollection<string> arguments);
    void SetNamedArguments(IReadOnlyDictionary<string, string> arguments);
  }
}