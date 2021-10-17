using MarkDoc.Core;

namespace MarkDoc.Plugins.GitMarkdown
{
  public abstract class BasePluginStep
    : IPluginStep
  {
    #region Properties

    /// <inheritdoc />
    public abstract string Name { get; }

    /// <inheritdoc />
    public abstract int StepNumber { get; }

    #endregion

    /// <inheritdoc />
    public abstract IView GetStepView(ILibrarySettings? settings = default);
  }
}