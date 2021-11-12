using System.Collections.Generic;
using System.Linq;
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
      => PluginManager.Plugins.Value.Values.ToArray();

    #region Commands

    /// <inheritdoc />
    public ICommand PluginNewCommand { get; }

    /// <inheritdoc />
    public ICommand PluginOpenCommand { get; }

    #endregion

    /// <summary>
    /// Default constructor
    /// </summary>
    public HomeViewModel(NavigationManager navigationManager)
    {
      m_navigationManager = navigationManager;

      PluginNewCommand = ReactiveCommand.Create<string>(PluginNew);
      PluginOpenCommand = ReactiveCommand.Create<string>(PluginOpen);
    }

    private void PluginNew(string pluginId)
      => m_navigationManager.NavigateTo(PageNames.CONFIGURATION, pluginId);

    private void PluginOpen(string pluginId)
      => m_navigationManager.NavigateTo(PageNames.CONFIGURATION, pluginId);
  }
}