using MarkDoc.Linkers;
using MarkDoc.Linkers.Markdown;

namespace MarkDoc.Plugins.GitMarkdown
{
  public sealed class LinkerStep : BasePluginStep
  {
    private readonly LinkerSettings m_settings;

    public override string Name => "Linker configuration";

    public override int StepNumber => 3;

    public LinkerStep(ILinkerSettings settings) => m_settings = (LinkerSettings) settings;
  }
}