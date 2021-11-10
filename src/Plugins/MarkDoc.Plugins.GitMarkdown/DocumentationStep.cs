using MarkDoc.Core;
using MarkDoc.Documentation;
using MarkDoc.MVVM.Helpers;

namespace MarkDoc.Plugins.GitMarkdown
{
  public sealed class DocumentationStep
    : BasePluginStep
  {
    /// <inheritdoc />
    public override string Name => "Documentation";

    /// <inheritdoc />
    public override int StepNumber => 2;

    /// <inheritdoc />
    public override IView GetStepView(ILibrarySettings? settings = default)
    {
      var view = TypeResolver.Resolve<IStepView<IStepViewModel<IDocSettings>, IDocSettings>>();

      return view;
    }
  }
}