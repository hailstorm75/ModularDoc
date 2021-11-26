using System.Collections.Generic;
using System.Threading.Tasks;
using MarkDoc.Core;
using MarkDoc.Members;
using MarkDoc.MVVM.Helpers;

namespace MarkDoc.Plugins.GitMarkdown
{
  public class AssembliesStep
    : BasePluginStep
  {
    /// <inheritdoc />
    public override string Id => "99B731AC-941C-42A6-BD77-AFADB3EC156B";

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
    public override async Task<IStepView<IStepViewModel>> GetStepView(IReadOnlyDictionary<string, string> settings,
      IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> previousSettings)
    {
      var view = TypeResolver.Resolve<IStepView<IStepViewModel<IMemberSettings>, IMemberSettings>>();
      await view.SetNamedArguments(settings);
      view.SetPreviousSettings(previousSettings);

      return view;
    }
  }
}