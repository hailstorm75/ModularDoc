using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using MarkDoc.Core;
using MarkDoc.Helpers;
using MarkDoc.Members;
using MarkDoc.MVVM.Helpers;
using ReactiveUI;

namespace MarkDoc.ViewModels.GitMarkdown
{
  public class GlobalStepViewModel
    : BaseStepViewModel<IGlobalSettings>
  {
    #region Fields

    private int m_totalTypes;
    private readonly HashSet<TypeNode> m_disabledTypeNodes = new();
    private readonly IDialogManager m_dialogManager;
    private readonly IResolver m_resolver;
    private readonly ISettingsCreator m_settingsCreator;
    private readonly List<BaseTrieNode> m_allNodes = new();
    private readonly TaskCompletionSource<IReadOnlyDictionary<string, string>> m_arguments = new();
    private string m_pathToOutput = string.Empty;

    #endregion

    #region Properties

    /// <inheritdoc />
    public override string Id => "23407B59-027B-43F9-901C-57F3016DE237";

    /// <inheritdoc />
    public override string Title => "Global settings";

    /// <inheritdoc />
    public override string Description => "Settings used by multiple components";

    /// <summary>
    /// Output directory
    /// </summary>
    public string PathToOutput
    {
      get => m_pathToOutput;
      set
      {
        m_pathToOutput = value;
        UpdateIsValid();
        OnPropertyChanged(nameof(PathToOutput));
      }
    }

    /// <summary>
    /// Namespaces inside the libraries
    /// </summary>
    public ObservableCollection<NamespaceNode> AvailableNamespaces { get; } = new();

    #endregion

    #region Commands

    public ICommand BrowseCommand { get; }

    #endregion

    /// <summary>
    /// Default constructor
    /// </summary>
    public GlobalStepViewModel(IDialogManager dialogManager, IResolver resolver, ISettingsCreator settingsCreator)
    {
      UpdateIsValid();

      BrowseCommand = ReactiveCommand.CreateFromTask(BrowseFolderAsync);

      m_dialogManager = dialogManager;
      m_resolver = resolver;
      m_settingsCreator = settingsCreator;
    }

    ~GlobalStepViewModel()
    {
      foreach (var space in AvailableNamespaces)
        space.PropertyChanged -= RootOnPropertyChanged;
    }

    #region Methods

    private async Task BrowseFolderAsync()
    {
      var option = await m_dialogManager.TrySelectFolderAsync("Select output");
      if (option.IsEmpty)
        return;

      PathToOutput = option.Get();
    }

    /// <inheritdoc />
    public override IReadOnlyDictionary<string, string> GetSettings()
    {
      var ignoredNamespaces = m_allNodes.OfType<NamespaceNode>()
        .Where(node => !node.IsEnabled || !node.IsChecked)
        .ToReadOnlyCollection();
      var ignoredTypes = m_allNodes.OfType<TypeNode>()
        .Where(node => !node.IsEnabled || !node.IsChecked)
        .ToReadOnlyCollection();

      return new Dictionary<string, string>
      {
        {
          nameof(IGlobalSettings.IgnoredNamespaces),
          string.Join(IGlobalSettings.DELIM, ignoredNamespaces.Select(node => node.FullName))
        },
        {
          nameof(IGlobalSettings.IgnoredTypes),
          string.Join(IGlobalSettings.DELIM, ignoredTypes.Select(node => node.FullName))
        },
        {
          nameof(IGlobalSettings.CheckedIgnoredNamespaces),
          string.Join(IGlobalSettings.DELIM, ignoredNamespaces.Where(node => node.IsChecked).Select(node => node.FullName))
        },
        {
          nameof(IGlobalSettings.CheckedIgnoredTypes),
          string.Join(IGlobalSettings.DELIM, ignoredTypes.Where(node => node.IsChecked).Select(node => node.FullName))
        },
        {
          nameof(IGlobalSettings.OutputPath),
          PathToOutput
        }
      };
    }

    /// <inheritdoc />
    public override Task SetNamedArguments(IReadOnlyDictionary<string, string> arguments)
    {
      m_arguments.TrySetResult(arguments);
      if (arguments.TryGetValue(nameof(IGlobalSettings.OutputPath), out var outputPath))
        // ReSharper disable once AssignNullToNotNullAttribute
        PathToOutput = outputPath;

      return Task.CompletedTask;
    }

    /// <inheritdoc />
    public override async ValueTask SetPreviousSettings(IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> settings)
    {
      await base.SetPreviousSettings(settings).ConfigureAwait(false);

      if (!settings.TryGetValue(AssemblyStepViewModel.ID, out var data))
        return;

      // ReSharper disable once RedundantSuppressNullableWarningExpression
      var memberSettings = m_settingsCreator.CreateSettings<IMemberSettings>(data!);
      var globalSettings = m_settingsCreator.CreateSettings<IGlobalSettings>(new Dictionary<string, string>());

      await m_resolver.ResolveAsync(memberSettings, globalSettings);

      var (roots, allNodes) = NamespaceNode.CreateNodes(m_resolver);
      m_allNodes.AddRange(allNodes);

      foreach (var root in roots)
      {
        AvailableNamespaces.Add(root);

        root.PropertyChanged += RootOnPropertyChanged;
      }

      foreach (var node in allNodes.OfType<TypeNode>())
      {
        ++m_totalTypes;

        node.PropertyChanged += TypeNodeOnPropertyChanging;
      }

      var arguments = await m_arguments.Task.ConfigureAwait(false);

      UpdateTreeNodeStates<NamespaceNode>(arguments, nameof(IGlobalSettings.IgnoredNamespaces), nameof(IGlobalSettings.CheckedIgnoredNamespaces));
      UpdateTreeNodeStates<TypeNode>(arguments, nameof(IGlobalSettings.IgnoredTypes), nameof(IGlobalSettings.CheckedIgnoredTypes));

      UpdateIsValid();
    }

    private void UpdateTreeNodeStates<T>(IReadOnlyDictionary<string, string> arguments, string ignored, string checkedIgnored)
      where T : BaseTrieNode
    {
      if (!arguments.TryGetValue(ignored, out var ignoredNodes))
        return;

      arguments.TryGetValue(checkedIgnored, out var checkedIgnoredSettings);
      var @checked = new HashSet<string>(checkedIgnoredSettings?.Split(IGlobalSettings.DELIM) ?? Array.Empty<string>());

      // ReSharper disable once PossibleNullReferenceException
      var types = ignoredNodes.Split(IGlobalSettings.DELIM).ToHashSet();
      var allTypes = m_allNodes.OfType<T>().Where(node => types.Contains(node.FullName));
      foreach (var node in allTypes)
      {
        node.IsChecked = @checked.Contains(node.FullName);
        node.IsEnabled = !node.IsChecked && (node.Parent?.IsChecked ?? false) && (node.Parent?.IsEnabled ?? false);
      }
    }

    private void TypeNodeOnPropertyChanging(object? sender, PropertyChangedEventArgs e)
    {
      var node = sender as TypeNode;

      switch (e.PropertyName)
      {
        case nameof(TypeNode.IsEnabled):
        case nameof(TypeNode.IsChecked):
          if (!node!.IsEnabled || !node.IsChecked)
            m_disabledTypeNodes.Add(node);
          else
            m_disabledTypeNodes.Remove(node);

          UpdateIsValid();
          break;
      }
    }

    private void RootOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
      switch (e.PropertyName)
      {
        case nameof(BaseTrieNode.IsChecked):
          UpdateIsValid();
          break;
      }
    }

    private static bool IsValidFolder(string folder)
      => !string.IsNullOrEmpty(folder) && Directory.Exists(folder);

    private void UpdateIsValid()
      => IsValid = IsValidFolder(PathToOutput) && AvailableNamespaces.Any(space => space.IsChecked) && m_totalTypes != m_disabledTypeNodes.Count;

    #endregion
  }
}