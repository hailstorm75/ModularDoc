using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarkDoc.Core
{
  /// <summary>
  /// Interface for views
  /// </summary>
  /// <typeparam name="TViewModel">View model type for the view</typeparam>
  public interface IView<out TViewModel>
    : IView
    where TViewModel : IViewModel
  {
    /// <summary>
    /// View model
    /// </summary>
    public TViewModel ViewModel { get; }
  }

  /// <summary>
  /// Interface for views
  /// </summary>
  public interface IView
  {
    /// <summary>
    /// Sets <paramref name="arguments"/> for the current view
    /// </summary>
    /// <param name="arguments">Arguments to set</param>
    public async Task SetArguments(IEnumerable<string> arguments)
      => await SetNamedArgumentsAsync(arguments.Select((x, i) => (x, i)).ToDictionary(el => $"{el.i}", el => el.x)).ConfigureAwait(false);

    /// <summary>
    /// Sets named <paramref name="arguments"/> for the current view
    /// </summary>
    /// <param name="arguments">Named arguments to set</param>
    Task SetNamedArgumentsAsync(IReadOnlyDictionary<string, string> arguments);
  }
}
