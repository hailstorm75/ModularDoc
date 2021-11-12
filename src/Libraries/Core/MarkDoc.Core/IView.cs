using System.Collections.Generic;
using System.Linq;

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
    public void SetArguments(IEnumerable<string> arguments)
      => SetNamedArguments(arguments.Select((x, i) => (x, i)).ToDictionary(el => $"{el.i}", el => el.x));

    /// <summary>
    /// Sets named <paramref name="arguments"/> for the current view
    /// </summary>
    /// <param name="arguments">Named arguments to set</param>
    void SetNamedArguments(IReadOnlyDictionary<string, string> arguments);
  }
}
