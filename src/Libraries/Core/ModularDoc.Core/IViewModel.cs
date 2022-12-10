using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ModularDoc.Core
{
  /// <summary>
  /// Interface for view models
  /// </summary>
  public interface IViewModel
    : IDisposable
  {
    /// <summary>
    /// Determines whether the view model is loading
    /// </summary>
    public bool IsLoading { get; }

    #region Methods

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

    #endregion
  }
}