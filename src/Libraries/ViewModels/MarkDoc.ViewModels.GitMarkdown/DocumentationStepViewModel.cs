using MarkDoc.Documentation;
using MarkDoc.MVVM.Helpers;

namespace MarkDoc.ViewModels.GitMarkdown
{
  public sealed class DocumentationStepViewModel
    : BaseStepViewModel<IDocSettings>
  {
    /// <inheritdoc />
    public override string Title => "Documentation extraction settings";

    /// <inheritdoc />
    public override string Description => "TODO";
  }
}