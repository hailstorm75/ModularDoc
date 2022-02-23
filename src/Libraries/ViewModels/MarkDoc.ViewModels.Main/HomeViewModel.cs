using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using DynamicData;
using DynamicData.Binding;
using MarkDoc.Constants;
using MarkDoc.Core;
using MarkDoc.Helpers;
using MarkDoc.MVVM.Helpers;
using Newtonsoft.Json;
using ReactiveUI;

namespace MarkDoc.ViewModels.Main
{
  public class HomeViewModel
    : BaseViewModel, IHomeViewModel
  {
    #region Fields

    private readonly NavigationManager m_navigationManager;
    private readonly IDialogManager m_dialogManager;
    private IPlugin? m_selectedPlugin;
    private bool m_paneOpen;
    private ReadOnlyObservableCollection<IPlugin> m_plugins;
    private string m_searchTerm = string.Empty;
    private readonly IDisposable m_cleanup;

    #endregion

    #region Properties

    /// <inheritdoc />
    public string SearchTerm
    {
      get => m_searchTerm;
      set
      {
        m_searchTerm = value;
        this.RaisePropertyChanged(nameof(SearchTerm));
      }
    }

    /// <inheritdoc />
    public bool PaneOpen
    {
      get => m_paneOpen;
      set
      {
        m_paneOpen = value;
        this.RaisePropertyChanged(nameof(PaneOpen));
      }
    }

    /// <inheritdoc />
    public IPlugin? SelectedPlugin
    {
      get => m_selectedPlugin;
      set
      {
        PaneOpen = true;
        m_selectedPlugin = value;

        this.RaisePropertyChanged(nameof(SelectedPlugin));
      }
    }

    /// <inheritdoc />
    public IReadOnlyCollection<IPlugin> Plugins
      => m_plugins;

    #endregion

    #region Commands

    /// <inheritdoc />
    public ICommand PluginNewCommand { get; }

    /// <inheritdoc />
    public ICommand PluginCancelCommand { get; }

    /// <inheritdoc />
    public ICommand PluginOpenCommand { get; }

    /// <inheritdoc />
    public ICommand OpenSettingsCommand { get; }

    /// <inheritdoc />
    public ICommand ClearSearchCommand { get; }

    #endregion

    /// <summary>
    /// Default constructor
    /// </summary>
    public HomeViewModel(NavigationManager navigationManager, IDialogManager dialogManager)
    {
      m_navigationManager = navigationManager;
      m_dialogManager = dialogManager;

      var canClearSearch = this
        .WhenAnyValue(x => x.SearchTerm)
        .Select(x => !string.IsNullOrEmpty(x));

      PluginNewCommand = ReactiveCommand.Create(PluginNew);
      PluginCancelCommand = ReactiveCommand.Create(PluginCancel);
      PluginOpenCommand = ReactiveCommand.CreateFromTask(PluginOpen);
      OpenSettingsCommand = ReactiveCommand.Create(NavigateToSettings);
      ClearSearchCommand = ReactiveCommand.Create(ClearSearch, canClearSearch);

      var filter = this.WhenValueChanged(t => t.SearchTerm)
        .Throttle(TimeSpan.FromMilliseconds(250))
        .Select(BuildFilter);

      var list = new SourceList<IPlugin>();
      list.AddRange(PluginManager.Plugins.Value.Values);
      m_cleanup = list.Connect()
        .Filter(filter)
        .Sort(SortExpressionComparer<IPlugin>.Ascending(plugin => plugin.Name), SortOptions.UseBinarySearch)
        .Bind(out m_plugins)
        .DisposeMany()
        .Subscribe();
    }

    #region Methods

    private static Func<IPlugin, bool> BuildFilter(string? searchText)
    {
      if (string.IsNullOrEmpty(searchText))
        return _ => true;

      return plugin => plugin.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                       plugin.Description.Contains(searchText, StringComparison.OrdinalIgnoreCase);
    }

    private void ClearSearch() => SearchTerm = string.Empty;

    private void PluginCancel()
    {
      SelectedPlugin = null;
      PaneOpen = false;
    }

    private void PluginNew()
      => m_navigationManager.NavigateTo(PageNames.CONFIGURATION, SelectedPlugin?.Id ?? string.Empty);

    private void NavigateToSettings()
      => m_navigationManager.NavigateTo(PageNames.SETTINGS);

    private async Task PluginOpen()
    {
      var configurationFile = await m_dialogManager.TrySelectFilesAsync("Open configuration", new [] { (new[] { "mconf" } as IEnumerable<string>, "Configuration") });
      if (configurationFile.IsEmpty)
        return;

      var paths = configurationFile.Get();
      if (paths.Count == 0)
        return;

      m_navigationManager.NavigateTo(PageNames.CONFIGURATION,
      new Dictionary<string, string>
      {
        { IConfiguratorViewModel.ARGUMENT_SETTINGS_PATH, paths.First() }
      });
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        m_cleanup.Dispose();
      }
    }

    #endregion
  }
}