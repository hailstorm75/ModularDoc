using System.Collections.Generic;
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
    public abstract IStepView<IStepViewModel> GetStepView(IReadOnlyDictionary<string, string> settings);

    /// <inheritdoc />
    public override string ToString() => Name;
  }
}