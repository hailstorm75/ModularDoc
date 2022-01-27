using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
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

    #endregion

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

    public IPlugin? SelectedPlugin
    {
      get => m_selectedPlugin;
      set
      {
        if (value is null)
          PaneOpen = false;
        else if (PaneOpen)
        {
          PaneOpen = false;
          PaneOpen = true;
        }
        else
          PaneOpen = true;

        m_selectedPlugin = value;

        this.RaisePropertyChanged(nameof(SelectedPlugin));
      }
    }

    /// <inheritdoc />
    public IReadOnlyCollection<IPlugin> Plugins
      => Enumerable.Repeat(PluginManager.Plugins.Value.Values.First(), 10).ToArray();

    #region Commands

    /// <inheritdoc />
    public ICommand PluginNewCommand { get; }

    /// <inheritdoc />
    public ICommand PluginOpenCommand { get; }

    /// <inheritdoc />
    public ICommand OpenSettingsCommand { get; }

    #endregion

    /// <summary>
    /// Default constructor
    /// </summary>
    public HomeViewModel(NavigationManager navigationManager, IDialogManager dialogManager)
    {
      m_navigationManager = navigationManager;
      m_dialogManager = dialogManager;

      PluginNewCommand = ReactiveCommand.Create(PluginNew);
      PluginOpenCommand = ReactiveCommand.CreateFromTask(PluginOpen);
      OpenSettingsCommand = ReactiveCommand.Create(NavigateToSettings);
    }

    private void PluginNew()
      => m_navigationManager.NavigateTo(PageNames.CONFIGURATION, SelectedPlugin?.Id ?? string.Empty);

    private void NavigateToSettings()
      => m_navigationManager.NavigateTo(PageNames.SETTINGS);

    private async Task PluginOpen()
    {
      var configurationFile = await m_dialogManager.TrySelectFilesAsync("Open configuration", new [] { (new[] { "conf" } as IEnumerable<string>, "Configuration") });
      if (configurationFile.IsEmpty)
        return;

      var paths = configurationFile.Get();
      if (paths.Count == 0)
        return;

      using var fileStream = new StreamReader(paths.First());
      var data = await fileStream.ReadToEndAsync().ConfigureAwait(false);
      var (pluginId, settings) = JsonConvert.DeserializeObject<KeyValuePair<string, string>>(data);

      m_navigationManager.NavigateTo(PageNames.CONFIGURATION,
      new Dictionary<string, string>
      {
        { IConfiguratorViewModel.ARGUMENT_ID, pluginId },
        { IConfiguratorViewModel.ARGUMENT_SETTINGS, settings }
      });
    }
  }
}