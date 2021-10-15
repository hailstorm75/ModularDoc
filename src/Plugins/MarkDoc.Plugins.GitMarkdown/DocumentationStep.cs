using MarkDoc.Documentation.Xml;
using MarkDoc.ViewModels.GitMarkdown;

namespace MarkDoc.Plugins.GitMarkdown
{
  public sealed class DocumentationStep
    : BasePluginStep<DocSettings, DocumentationStepViewModel>
  {
    /// <inheritdoc />
    public override string Name => "Documentation configuration";

    /// <inheritdoc />
    public override int StepNumber => 2;
  }
}