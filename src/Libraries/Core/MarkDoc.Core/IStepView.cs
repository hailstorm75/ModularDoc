using System.Collections.Generic;

namespace MarkDoc.Core
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
    /// Sets previous step settings
    /// </summary>
    /// <param name="settings">Previous settings</param>
    public void SetPreviousSettings(IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> settings)
      => ViewModel.SetPreviousSettings(settings);
  }
}