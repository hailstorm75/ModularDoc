using System.Collections.Generic;
using System.Threading.Tasks;

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
    Task SetNamedArguments(IReadOnlyDictionary<string, string> arguments);

    /// <summary>
    /// Executed when the <see cref="IView{TViewModel}"/> loads
    /// </summary>
    /// <returns></returns>
    ValueTask OnLoadedAsync();
  }
}