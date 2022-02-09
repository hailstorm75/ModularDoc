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
    private bool m_linksToSourceCode;
    private string m_gitUser = string.Empty;
    private string m_gitBranch = string.Empty;
    private string m_gitRepository = string.Empty;

    /// <inheritdoc />
    public override string Id => "91ACE811-A3E5-4C43-9B7F-0721A3365E93";

    /// <inheritdoc />
    public override string Title => "Linking settings";

    /// <inheritdoc />
    public override string Description => "Select the target platform and relevant settings for correct link generation";

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
    /// Toggles linking members to lines in source code
    /// </summary>
    public bool LinksToSourceCode
    {
      get => m_linksToSourceCode;
      set
      {
        m_linksToSourceCode = value;
        OnPropertyChanged();
      }
    }

    /// <summary>
    /// Git repository username
    /// </summary>
    public string GitUser
    {
      get => m_gitUser;
      set
      {
        m_gitUser = value;
        OnPropertyChanged();
      }
    }

    /// <summary>
    /// Git branch name
    /// </summary>
    public string GitBranch
    {
      get => m_gitBranch;
      set
      {
        m_gitBranch = value;
        OnPropertyChanged();
      }
    }

    /// <summary>
    /// Git repository name
    /// </summary>
    public string GitRepository
    {
      get => m_gitRepository;
      set
      {
        m_gitRepository = value;
        OnPropertyChanged();
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
      if (!arguments.TryGetValue(LinkerSettings.ENTRY_PLATFORM, out var data)
          || !arguments.TryGetValue(LinkerSettings.PLATFORM_USER, out var gitUser)
          || !arguments.TryGetValue(LinkerSettings.PLATFORM_REPOSITORY, out var gitRepository)
          || !arguments.TryGetValue(LinkerSettings.PLATFORM_BRANCH, out var gitBranch)
          || !arguments.TryGetValue(LinkerSettings.ENABLE_LINKS_TO_SOURCE, out var linkSources)
          || !bool.TryParse(linkSources, out var linksToSourceCode))
        return Task.CompletedTask;

      Selected = Options.First(x => x.Value.ToString().Equals(data));
      // ReSharper disable AssignNullToNotNullAttribute
      GitUser = gitUser;
      GitRepository = gitRepository;
      GitBranch = gitBranch;
      // ReSharper restore AssignNullToNotNullAttribute
      LinksToSourceCode = linksToSourceCode;

      return Task.CompletedTask;
    }

    /// <inheritdoc />
    public override IReadOnlyDictionary<string, string> GetSettings()
      => new Dictionary<string, string>
      {
        {
          LinkerSettings.ENTRY_PLATFORM, Selected!.Value.Value.ToString()
        },
        {
          LinkerSettings.PLATFORM_USER, GitUser
        },
        {
          LinkerSettings.PLATFORM_BRANCH, GitBranch
        },
        {
          LinkerSettings.PLATFORM_REPOSITORY, GitRepository
        },
        {
          LinkerSettings.ENABLE_LINKS_TO_SOURCE, LinksToSourceCode.ToString()
        }
      };
  }
}