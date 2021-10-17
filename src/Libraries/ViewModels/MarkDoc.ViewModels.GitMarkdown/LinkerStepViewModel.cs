using MarkDoc.Linkers;
using MarkDoc.MVVM.Helpers;

namespace MarkDoc.ViewModels.GitMarkdown
{
  public sealed class LinkerStepViewModel
    : BaseStepViewModel<ILinkerSettings>
  {
    /// <inheritdoc />
    public override string Title => "Linking settings";

    /// <inheritdoc />
    public override string Description => "";
  }
}