using System.Collections.Generic;

namespace MarkDoc.Core
{
  /// <summary>
  /// Interface for view models
  /// </summary>
  public interface IViewModel
  {
    void SetNamedArguments(IReadOnlyDictionary<string, string> arguments);
  }
}