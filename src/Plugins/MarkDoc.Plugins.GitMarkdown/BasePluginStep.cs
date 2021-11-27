using System.Collections.Generic;
using System.Threading.Tasks;
using MarkDoc.Core;
using MarkDoc.MVVM.Helpers;

namespace MarkDoc.Plugins.GitMarkdown
{
  public abstract class BasePluginStep
    : IPluginStep
  {
    #region Properties

    /// <inheritdoc />
    public abstract string Id { get; }

    /// <inheritdoc />
    public abstract string Name { get; }

    /// <inheritdoc />
    public abstract int StepNumber { get; }

    /// <inheritdoc />
    public abstract bool IsLastStep { get; }

    #endregion

    /// <inheritdoc />
    public abstract string GetViewId();

    /// <inheritdoc />
    public abstract Task<IStepView<IStepViewModel>> GetStepViewAsync(IReadOnlyDictionary<string, string> settings, IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> previousSettings);

    protected static async Task<IStepView<IStepViewModel>> GetStepViewAsync<T>(
      IReadOnlyDictionary<string, string> settings,
      IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> previousSettings)
      where T : ILibrarySettings
    {
      var view = TypeResolver.Resolve<IStepView<IStepViewModel<T>, T>>();
      await Task.WhenAll(view.SetNamedArgumentsAsync(settings), view.SetPreviousSettingsAsync(previousSettings)).ConfigureAwait(false);

      return view;
    }

    /// <inheritdoc />
    public override string ToString() => Name;
  }
}