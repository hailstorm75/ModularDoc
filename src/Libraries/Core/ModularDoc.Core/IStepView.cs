using System.Collections.Generic;
using System.Threading.Tasks;

namespace ModularDoc.Core
{
  /// <summary>
  /// Interface for views of plugin steps
  /// </summary>
  /// <typeparam name="TViewModel">View model type for the view</typeparam>
  /// <typeparam name="TSettings">Step settings type</typeparam>
  public interface IStepView<out TViewModel, TSettings>
    :  IStepView<TViewModel>
    where TSettings : ILibrarySettings
    where TViewModel : IStepViewModel<TSettings>
  {
  }

  /// <summary>
  /// Interface for views of plugin steps
  /// </summary>
  /// <typeparam name="TViewModel">View model type for the view</typeparam>
  public interface IStepView<out TViewModel>
    : IView<TViewModel>
    where TViewModel : IStepViewModel
  {
    /// <summary>
    /// Step view Id
    /// </summary>
    public string Id => ViewModel.Id;

    /// <summary>
    /// Sets previous step settings
    /// </summary>
    /// <param name="settings">Previous settings</param>
    public async Task SetPreviousSettingsAsync(IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> settings)
      => await ViewModel.SetPreviousSettings(settings).ConfigureAwait(false);
  }
}