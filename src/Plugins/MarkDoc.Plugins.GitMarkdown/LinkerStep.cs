using MarkDoc.Core;
using MarkDoc.Linkers;
using MarkDoc.MVVM.Helpers;

namespace MarkDoc.Plugins.GitMarkdown
{
  public sealed class LinkerStep
    : BasePluginStep
  {
    /// <inheritdoc />
    public override string Name => "Linker";

    /// <inheritdoc />
    public override int StepNumber => 3;

    /// <inheritdoc />
    public override IView GetStepView(ILibrarySettings? settings = default)
    {
      var view = TypeResolver.Resolve<IStepView<IStepViewModel<ILinkerSettings>, ILinkerSettings>>();

      return view;
    }
  }
}