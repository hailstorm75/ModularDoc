using MarkDoc.Documentation.Xml;
using MarkDoc.ViewModels.GitMarkdown;

namespace MarkDoc.Plugins.GitMarkdown
{
  public sealed class DocumentationStep
    : BasePluginStep<DocSettings, DocumentationStepViewModel>
  {
    public override string Name => "Documentation configuration";

    public override int StepNumber => 2;
  }
}