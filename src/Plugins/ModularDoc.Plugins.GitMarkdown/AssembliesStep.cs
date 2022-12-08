using System.Collections.Generic;
using System.Threading.Tasks;
using ModularDoc;
using ModularDoc.Members;
using ModularDoc.MVVM.Helpers;

namespace ModularDoc.Plugins.GitMarkdown
{
  public class AssembliesStep
    : BasePluginStep
  {
    public const string ID = "99B731AC-941C-42A6-BD77-AFADB3EC156B";

    /// <inheritdoc />
    public override string Id => ID;

    /// <inheritdoc />
    public override string Name => "Assemblies";

    /// <inheritdoc />
    public override int StepNumber => 1;

    /// <inheritdoc />
    public override bool IsLastStep => false;

    /// <inheritdoc />
    public override string GetViewId()
    {
      var view = TypeResolver.Resolve<IStepView<IStepViewModel<IMemberSettings>, IMemberSettings>>();
      return view.Id;
    }

    /// <inheritdoc />
    public override async Task<IStepView<IStepViewModel>> GetStepViewAsync(IReadOnlyDictionary<string, string> settings, IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> previousSettings)
      => await GetStepViewAsync<IMemberSettings>(settings, previousSettings).ConfigureAwait(false);
  }
}