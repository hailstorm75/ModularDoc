using MarkDoc.Members.Dnlib;
using MarkDoc.ViewModels.GitMarkdown;

namespace MarkDoc.Plugins.GitMarkdown
{
  public class AssembliesStep
    : BasePluginStep<MemberSettings, AssemblyStepViewModel>
  {
    public override string Name => "Assemblies configuration";

    public override int StepNumber => 1;
  }
}