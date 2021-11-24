using System.Collections.Generic;
using MarkDoc.Core;
using MarkDoc.Linkers;
using MarkDoc.MVVM.Helpers;

namespace MarkDoc.Plugins.GitMarkdown
{
  public sealed class LinkerStep
    : BasePluginStep
  {
    /// <inheritdoc />
    public override string Id => "B05E71DB-7CB9-4855-8B4D-9A334677FEEB";

    /// <inheritdoc />
    public override string Name => "Linker";

    /// <inheritdoc />
    public override int StepNumber => 3;

    /// <inheritdoc />
    public override bool IsLastStep => true;

    /// <inheritdoc />
    public override IView GetStepView(IReadOnlyDictionary<string, string> settings)
    {
      var view = TypeResolver.Resolve<IStepView<IStepViewModel<ILinkerSettings>, ILinkerSettings>>();
      view.SetNamedArguments(settings);

      return view;
    }
  }
}