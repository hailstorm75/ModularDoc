using System.Collections.Generic;
using System.Windows.Input;
using MarkDoc.Constants;
using MarkDoc.Core;
using MarkDoc.MVVM.Helpers;
using ReactiveUI;

namespace MarkDoc.ViewModels.Main
{
  public class HomeViewModel
    : BaseViewModel, IHomeViewModel
  {
    private readonly NavigationManager m_navigationManager;

    /// <inheritdoc />
    public IReadOnlyCollection<IPlugin> Plugins
      => PluginManagers.Plugins;

    /// <inheritdoc />
    public ICommand PluginSelectedCommand { get; }

    /// <summary>
    /// Default constructor
    /// </summary>
    public HomeViewModel(NavigationManager navigationManager)
    {
      m_navigationManager = navigationManager;
      PluginSelectedCommand = ReactiveCommand.Create<string>(PluginSelected);
    }

    private void PluginSelected(string pluginId)
      => m_navigationManager.NavigateTo(PageNames.CONFIGURATION, pluginId);
  }
}