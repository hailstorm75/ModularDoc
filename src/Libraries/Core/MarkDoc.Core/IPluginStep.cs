namespace MarkDoc.Core
{
  public interface IPluginStep<TSettings, out TStepViewModel>
    : IPluginStep
    where TSettings : ILibrarySettings
    where TStepViewModel : IStepViewModel<TSettings>
  {
    IStepView<TStepViewModel, TSettings> GetStepView(TSettings? settings = default);
  }

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