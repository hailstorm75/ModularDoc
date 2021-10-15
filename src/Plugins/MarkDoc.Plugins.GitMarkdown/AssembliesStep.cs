using MarkDoc.Members.Dnlib;
using MarkDoc.ViewModels.GitMarkdown;

namespace MarkDoc.Plugins.GitMarkdown
{
  public class AssembliesStep
    : BasePluginStep<MemberSettings, AssemblyStepViewModel>
  {
    /// <inheritdoc />
    public override string Name => "Assemblies configuration";

    /// <inheritdoc />
    public override int StepNumber => 1;
  }
}