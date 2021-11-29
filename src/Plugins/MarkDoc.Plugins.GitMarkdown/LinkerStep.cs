using System.Collections.Generic;
using System.Threading.Tasks;
using MarkDoc.Core;
using MarkDoc.Linkers;
using MarkDoc.MVVM.Helpers;

namespace MarkDoc.Plugins.GitMarkdown
{
  public sealed class LinkerStep
    : BasePluginStep
  {
    public const string ID = "B05E71DB-7CB9-4855-8B4D-9A334677FEEB";

    /// <inheritdoc />
    public override string Id => ID;

    /// <inheritdoc />
    public override string Name => "Linker";

    /// <inheritdoc />
    public override int StepNumber => 3;

    /// <inheritdoc />
    public override bool IsLastStep => false;

    /// <inheritdoc />
    public override string GetViewId()
    {
      var view = TypeResolver.Resolve<IStepView<IStepViewModel<ILinkerSettings>, ILinkerSettings>>();
      return view.Id;
    }

    /// <inheritdoc />
    public override async Task<IStepView<IStepViewModel>> GetStepViewAsync(IReadOnlyDictionary<string, string> settings, IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> previousSettings)
      => await GetStepViewAsync<ILinkerSettings>(settings, previousSettings).ConfigureAwait(false);
  }
}