using System.Collections.Generic;
using MarkDoc.Core;
using MarkDoc.Documentation;
using MarkDoc.MVVM.Helpers;

namespace MarkDoc.Plugins.GitMarkdown
{
  public sealed class DocumentationStep
    : BasePluginStep
  {
    /// <inheritdoc />
    public override string Id => "A59A49C1-F12E-40D5-AE2B-EFEBFF76B10D";

    /// <inheritdoc />
    public override string Name => "Documentation";

    /// <inheritdoc />
    public override int StepNumber => 2;

    /// <inheritdoc />
    public override IView GetStepView(IReadOnlyDictionary<string, string> settings)
    {
      var view = TypeResolver.Resolve<IStepView<IStepViewModel<IDocSettings>, IDocSettings>>();
      view.SetNamedArguments(settings);

      return view;
    }
  }
}