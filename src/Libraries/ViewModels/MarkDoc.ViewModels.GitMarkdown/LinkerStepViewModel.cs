using System.Collections.Generic;
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

    /// <inheritdoc />
    public override void SetNamedArguments(IReadOnlyDictionary<string, string> arguments)
    {
      // throw new System.NotImplementedException();
    }
  }
}