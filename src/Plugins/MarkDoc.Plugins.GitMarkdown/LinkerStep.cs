using MarkDoc.Linkers.Markdown;
using MarkDoc.ViewModels.GitMarkdown;

namespace MarkDoc.Plugins.GitMarkdown
{
  public sealed class LinkerStep
    : BasePluginStep<LinkerSettings, LinkerStepViewModel>
  {
    /// <inheritdoc />
    public override string Name => "Linker configuration";

    /// <inheritdoc />
    public override int StepNumber => 3;
  }
}