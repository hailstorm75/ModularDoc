using System.Collections.Generic;
using MarkDoc.Linkers;
using MarkDoc.MVVM.Helpers;

namespace MarkDoc.ViewModels.GitMarkdown
{
  public sealed class LinkerStepViewModel
    : BaseStepViewModel<ILinkerSettings>
  {
    /// <inheritdoc />
    public override string Id => "91ACE811-A3E5-4C43-9B7F-0721A3365E93";

    /// <inheritdoc />
    public override string Title => "Linking settings";

    /// <inheritdoc />
    public override string Description => "";

    /// <inheritdoc />
    public override void SetNamedArguments(IReadOnlyDictionary<string, string> arguments)
    {
      // throw new System.NotImplementedException();
    }

    /// <inheritdoc />
    public override IReadOnlyDictionary<string, string> GetSettings()
      => new Dictionary<string, string>();
  }
}