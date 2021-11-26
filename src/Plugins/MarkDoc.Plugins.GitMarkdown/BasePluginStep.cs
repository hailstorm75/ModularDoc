using System.Collections.Generic;
using System.Threading.Tasks;
using MarkDoc.Core;

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
    public abstract Task<IStepView<IStepViewModel>> GetStepView(IReadOnlyDictionary<string, string> settings,
      IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> previousSettings);

    /// <inheritdoc />
    public override string ToString() => Name;
  }
}