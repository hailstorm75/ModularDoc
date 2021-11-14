using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
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
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public ICommand BrowseCommand { get; }

    /// <summary>
    /// Command for adding a new path
    /// </summary>
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public ReactiveCommand<Unit, bool> AddCommand { get; }

    /// <summary>
    /// Command for removing a path
    /// </summary>
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public ICommand RemoveCommand { get; }

    #endregion

    /// <summary>
    /// Default constructor
    /// </summary>
    public AssemblyStepViewModel(IDialogManager dialogManager)
    {
      var canExecuteChanged = this
        .WhenAnyValue(viewModel => viewModel.PathToAssembly)
        .Select(IsValidPath);

      AddCommand = ReactiveCommand.Create(AddPath, canExecuteChanged);
      BrowseCommand = ReactiveCommand.CreateFromTask(BrowseAsync);
      RemoveCommand = ReactiveCommand.Create<string>(RemovePath);
      m_dialogManager = dialogManager;
    }

    #region Methods

    private bool IsValidPath(string? path)
    {
      var trimmed = path?.Trim() ?? string.Empty;

      return !string.IsNullOrEmpty(trimmed)
             && !m_pathsInsensitive.Contains(trimmed)
             && trimmed.EndsWith(".dll")
             && File.Exists(trimmed);
    }

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

      this.RaisePropertyChanging(nameof(Paths));
      UpdateCanProceed();

      return true;
    }

    private void RemovePath(string path)
    {
      if (!m_pathsInsensitive.Contains(path))
        return;

      m_pathsInsensitive.Remove(path);
      Paths.Remove(path);

      this.RaisePropertyChanging(nameof(Paths));
      UpdateCanProceed();
    }

    private void UpdateCanProceed()
      => OnCanProceed(Paths.Count > 0);

    #endregion
  }
}