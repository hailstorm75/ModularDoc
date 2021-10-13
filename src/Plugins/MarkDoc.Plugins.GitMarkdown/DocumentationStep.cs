using MarkDoc.Documentation;

namespace MarkDoc.Plugins.GitMarkdown
{
  public sealed class DocumentationStep : BasePluginStep
  {
    private readonly IDocSettings m_settings;

    public override string Name => "Documentation configuration";

    public override int StepNumber => 2;

    public DocumentationStep(IDocSettings settings) => m_settings = settings;
  }
}