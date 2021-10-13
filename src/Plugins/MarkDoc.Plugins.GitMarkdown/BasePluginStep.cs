using MarkDoc.Core;

namespace MarkDoc.Plugins.GitMarkdown
{
  public abstract class BasePluginStep
    : IPluginStep
  {
    public abstract string Name { get; }
    public abstract int StepNumber { get; }
  }
}