using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using DynamicData;
using MarkDoc.Helpers;
using MarkDoc.Members;
using MarkDoc.MVVM.Helpers;
using ReactiveUI;

namespace MarkDoc.ViewModels.GitMarkdown
{
  public sealed class AssemblyStepViewModel
    : BaseStepViewModel<IMemberSettings>
  {
    #region Constants

    private const string SETTINGS_PATHS = "paths";
    private const char PATH_DELIM = '|';

    #endregion

    private readonly HashSet<string> m_pathsInsensitive = new (StringComparer.InvariantCultureIgnoreCase);
    private readonly IDialogManager m_dialogManager;
    private string m_pathToAssembly = string.Empty;

    #region Properties

    /// <inheritdoc />
    public override string Title => "Assembly processing settings";

    /// <inheritdoc />
    public override string Description => "TODO";

    public string PathToAssembly
    {
      get => m_pathToAssembly;
      set
      {
        m_pathToAssembly = value;
        this.RaisePropertyChanged(nameof(PathToAssembly));
      }
    }

    public ObservableCollection<string> Paths { get; } = new();

    #endregion

    #region Commands

    /// <summary>
    /// Command for browsing for assembly files
    /// </summary>
    public ICommand BrowseCommand { get; }

    /// <summary>
    /// Command for adding a new path
    /// </summary>
    public ICommand AddCommand { get; }

    #endregion

    /// <summary>
    /// Default constructor
    /// </summary>
    public AssemblyStepViewModel(IDialogManager dialogManager)
    {
      AddCommand = ReactiveCommand.Create(AddPath);
      BrowseCommand = ReactiveCommand.CreateFromTask(BrowseAsync);
      m_dialogManager = dialogManager;
    }

    #region Methods

    /// <inheritdoc />
    public override void SetNamedArguments(IReadOnlyDictionary<string, string> arguments)
    {
      if (!arguments.TryGetValue(SETTINGS_PATHS, out var pathsSerialized))
        return;

      // ReSharper disable once PossibleNullReferenceException
      Paths.AddRange(pathsSerialized.Split(PATH_DELIM));
    }

    private async Task BrowseAsync()
    {
      var result = await m_dialogManager.TrySelectFilesAsync("Select a .NET assembly", new [] { (new[] { "dll" } as IEnumerable<string>, "Assembly") });
      if (result.IsEmpty)
        return;

      PathToAssembly = result.Get().First();
    }

    private bool AddPath()
    {
      var path = PathToAssembly;

      if (m_pathsInsensitive.Contains(path))
        return false;

      m_pathsInsensitive.Add(path);
      Paths.Add(path);

      UpdateCanProceed();

      return true;
    }

    private bool RemovePath(string path)
    {
      if (!m_pathsInsensitive.Contains(path))
        return false;

      m_pathsInsensitive.Remove(path);
      Paths.Remove(path);

      UpdateCanProceed();

      return true;
    }

    private void UpdateCanProceed()
      => OnCanProceed(Paths.Count > 0);

    #endregion
  }
}