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

    public const string ID = "697C851D-54B1-4D84-BFD7-3568F54BB2C5";

    #endregion

    private readonly HashSet<string> m_pathsInsensitive = new (StringComparer.InvariantCultureIgnoreCase);
    private readonly IDialogManager m_dialogManager;
    private string m_pathToAssembly = string.Empty;

    #region Properties

    /// <inheritdoc />
    public override string Id => ID;

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
        OnPropertyChanged();
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
    /// Command for adding multiple new paths
    /// </summary>
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public ICommand AddMultipleCommand { get; }

    /// <summary>
    /// Command for validating present commands
    /// </summary>
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public ICommand ValidateCommand { get; }

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
      var canExecuteAddPath = this
        .WhenAnyValue(viewModel => viewModel.PathToAssembly)
        .Select(IsValidPath);

      var canExecuteValidatePaths = this
        .WhenAnyValue(viewModel => viewModel.Paths)
        .Select(x => x.Count > 0);

      AddCommand = ReactiveCommand.Create(AddPath, canExecuteAddPath);
      AddMultipleCommand = ReactiveCommand.CreateFromTask(AddMultiplePaths);
      ValidateCommand = ReactiveCommand.Create(ValidatePaths, canExecuteValidatePaths);
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
    public override Task SetNamedArguments(IReadOnlyDictionary<string, string> arguments)
    {
      if (!arguments.TryGetValue(IMemberSettings.ENTRY_PATHS, out var pathsSerialized))
        return Task.CompletedTask;

      // ReSharper disable once PossibleNullReferenceException
      Paths.AddRange(pathsSerialized.Split(IMemberSettings.PATH_DELIMITER));
      m_pathsInsensitive.AddRange(Paths);

      UpdateCanProceed();
      return Task.CompletedTask;
    }

    /// <inheritdoc />
    public override IReadOnlyDictionary<string, string> GetSettings()
      => new Dictionary<string, string>
      {
        {
          IMemberSettings.ENTRY_PATHS, string.Join(IMemberSettings.PATH_DELIMITER, Paths)
        }
      };

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
      Paths.AddSorted(path);

      OnPropertyChanged(nameof(Paths));
      UpdateCanProceed();

      return true;
    }

    private async Task AddMultiplePaths()
    {
      var result = await m_dialogManager.TrySelectFilesAsync("Select .NET assemblies", new[] { (new[] { "dll" } as IEnumerable<string>, "Assembly") }, true);
      if (result.IsEmpty)
        return;

      foreach (var path in result.Get())
      {
        if (m_pathsInsensitive.Contains(path))
          continue;

        m_pathsInsensitive.Add(path);
        Paths.AddSorted(path);
      }

      OnPropertyChanged(nameof(Paths));
      UpdateCanProceed();
    }

    private void ValidatePaths()
    {
      var toRemove = Paths.Where(path => !File.Exists(path)).ToReadOnlyCollection();

      // TODO: Confirm dialog with list of paths
      foreach (var path in toRemove)
        Paths.Remove(path);
    }

    private void RemovePath(string path)
    {
      if (!m_pathsInsensitive.Contains(path))
        return;

      m_pathsInsensitive.Remove(path);
      Paths.Remove(path);

      OnPropertyChanged(nameof(Paths));
      UpdateCanProceed();
    }

    private void UpdateCanProceed()
      => IsValid = Paths.Count > 0;

    #endregion
  }
}