using MarkDoc.Linkers.Markdown;
using MarkDoc.ViewModels.GitMarkdown;

namespace MarkDoc.Plugins.GitMarkdown
{
  public sealed class LinkerStep
    : BasePluginStep<LinkerSettings, LinkerStepViewModel>
  {
    public override string Name => "Linker configuration";

    public override int StepNumber => 3;
  }
}