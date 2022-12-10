using System.Collections.Generic;
using System.Threading.Tasks;
using ModularDoc;
using ModularDoc.Core;
using ModularDoc.Documentation;
using ModularDoc.MVVM.Helpers;

namespace ModularDoc.Plugins.GitMarkdown
{
  public sealed class DocumentationStep
    : BasePluginStep
  {
    public const string ID = "A59A49C1-F12E-40D5-AE2B-EFEBFF76B10D";

    /// <inheritdoc />
    public override string Id => ID;

    /// <inheritdoc />
    public override string Name => "Documentation";

    /// <inheritdoc />
    public override int StepNumber => 2;

    /// <inheritdoc />
    public override bool IsLastStep => false;

    /// <inheritdoc />
    public override string GetViewId()
    {
      var view = TypeResolver.Resolve<IStepView<IStepViewModel<IDocSettings>, IDocSettings>>();
      return view.Id;
    }

    /// <inheritdoc />
    public override async Task<IStepView<IStepViewModel>> GetStepViewAsync(IReadOnlyDictionary<string, string> settings, IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> previousSettings)
      => await GetStepViewAsync<IDocSettings>(settings, previousSettings).ConfigureAwait(false);
  }
}