using MarkDoc.Core;
using MarkDoc.Members;
using MarkDoc.MVVM.Helpers;

namespace MarkDoc.Plugins.GitMarkdown
{
  public class AssembliesStep
    : BasePluginStep
  {
    /// <inheritdoc />
    public override string Name => "Assemblies";

    /// <inheritdoc />
    public override int StepNumber => 1;

    /// <inheritdoc />
    public override IView GetStepView(ILibrarySettings? settings = default)
    {
      var view = TypeResolver.Resolve<IStepView<IStepViewModel<IMemberSettings>, IMemberSettings>>();

      return view;
    }
  }
}