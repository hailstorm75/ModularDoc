using System.Collections.Generic;

namespace MarkDoc.Core
{
  /// <summary>
  /// Interface for view models
  /// </summary>
  public interface IViewModel
  {
    /// <summary>
    /// Sets named <paramref name="arguments"/> for the view model
    /// </summary>
    /// <param name="arguments">Named arguments to set</param>
    void SetNamedArguments(IReadOnlyDictionary<string, string> arguments);
  }
}