using MarkDoc.Members;
using MarkDoc.MVVM.Helpers;

namespace MarkDoc.ViewModels.GitMarkdown
{
  public sealed class AssemblyStepViewModel
    : BaseStepViewModel<IMemberSettings>
  {
    /// <inheritdoc />
    public override string Title => "Assembly processing settings";

    /// <inheritdoc />
    public override string Description => "TODO";
  }
}