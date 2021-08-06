using System.Collections.Generic;
using System.Linq;

namespace MarkDoc.ViewModels
{
  /// <summary>
  /// Interface for view models
  /// </summary>
  public interface IViewModel
  {
    public void SetArguments(IEnumerable<string> arguments)
      => SetNamedArguments(arguments.ToDictionary(arg => arg, _ => string.Empty));

    void SetNamedArguments(IReadOnlyDictionary<string, string> arguments);
  }
}