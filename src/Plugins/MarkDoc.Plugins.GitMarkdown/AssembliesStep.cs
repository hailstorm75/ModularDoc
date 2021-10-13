using MarkDoc.Members;

namespace MarkDoc.Plugins.GitMarkdown
{
  public class AssembliesStep
    : BasePluginStep
  {
    private readonly IMemberSettings m_settings;

    public override string Name => "Assemblies configuration";

    public override int StepNumber => 1;

    public AssembliesStep(IMemberSettings settings) => m_settings = settings;
  }
}