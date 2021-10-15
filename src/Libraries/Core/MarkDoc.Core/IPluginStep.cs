namespace MarkDoc.Core
{
  /// <summary>
  /// Interface for plugin steps
  /// </summary>
  /// <typeparam name="TSettings">Step settings type</typeparam>
  /// <typeparam name="TStepViewModel">View model type for the step view</typeparam>
  public interface IPluginStep<TSettings, out TStepViewModel>
    : IPluginStep
    where TSettings : ILibrarySettings
    where TStepViewModel : IStepViewModel<TSettings>
  {
    /// <summary>
    /// Gets the view for this step
    /// </summary>
    /// <param name="settings">Settings to use</param>
    /// <returns>View instance</returns>
    IStepView<TStepViewModel, TSettings> GetStepView(TSettings? settings = default);
  }

  /// <summary>
  /// Interface for plugin steps
  /// </summary>
  public interface IPluginStep
  {
    /// <summary>
    /// Step name
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Step order number
    /// </summary>
    int StepNumber { get; }
  }
}