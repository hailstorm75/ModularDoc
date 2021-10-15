using System;
using MarkDoc.Core;

namespace MarkDoc.Plugins.GitMarkdown
{
  public abstract class BasePluginStep<TSettings, TStepViewModel>
    : IPluginStep<TSettings, TStepViewModel>
    where TSettings : ILibrarySettings
    where TStepViewModel : IStepViewModel<TSettings>
  {
    #region Properties

    /// <inheritdoc />
    public abstract string Name { get; }

    /// <inheritdoc />
    public abstract int StepNumber { get; }

    #endregion

    /// <inheritdoc />
    public IStepView<TStepViewModel, TSettings> GetStepView(TSettings? settings = default)
    {
      throw new NotImplementedException();
    }
  }
}