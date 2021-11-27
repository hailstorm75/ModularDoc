using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using MarkDoc.Linkers;
using MarkDoc.Linkers.Markdown;
using MarkDoc.MVVM.Helpers;

namespace MarkDoc.ViewModels.GitMarkdown
{
  public sealed class LinkerStepViewModel
    : BaseStepViewModel<ILinkerSettings>
  {
    private Selection? m_selected;

    /// <inheritdoc />
    public override string Id => "91ACE811-A3E5-4C43-9B7F-0721A3365E93";

    /// <inheritdoc />
    public override string Title => "Linking settings";

    /// <inheritdoc />
    public override string Description => "TODO";

    public ObservableCollection<Selection> Options { get; } = new();

    public Selection? Selected
    {
      get => m_selected;
      set
      {
        m_selected = value;
        OnPropertyChanged(nameof(Selected));
      }
    }

    /// <summary>
    /// Default constructor
    /// </summary>
    public LinkerStepViewModel()
    {
      IsValid = true;

      Options.Add(new Selection("GitHub", (int)GitPlatform.GitHub));
      Options.Add(new Selection("GitLab", (int)GitPlatform.GitLab));

      Selected = Options.First();
    }

    /// <inheritdoc />
    public override Task SetNamedArguments(IReadOnlyDictionary<string, string> arguments)
    {
      if (!arguments.TryGetValue(LinkerSettings.ENTRY_PLATFORM, out var data))
        return Task.CompletedTask;

      Selected = Options.First(x => x.Value.ToString().Equals(data));
      return Task.CompletedTask;
    }

    /// <inheritdoc />
    public override IReadOnlyDictionary<string, string> GetSettings()
      => new Dictionary<string, string>
      {
        {
          LinkerSettings.ENTRY_PLATFORM, Selected!.Value.Value.ToString()
        }
      };
  }
}