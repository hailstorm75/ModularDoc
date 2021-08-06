using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using MarkDoc.Constants;
using MarkDoc.Core;
using MarkDoc.MVVM.Helpers;
using ReactiveUI;

namespace MarkDoc.ViewModels.Main
{
  public class ConfiguratorViewModel
    : BaseViewModel, IConfiguratorViewModel
  {
    #region Fields

    private readonly NavigationManager m_navigationManager;
    private IPlugin? m_plugin;

    #endregion

    #region Properties

    /// <inheritdoc />
    public string Title => "Plugin: " + (m_plugin?.Name ?? string.Empty);

    #endregion

    #region Commands

    /// <inheritdoc />
    public ICommand BackCommand { get; }

    #endregion

    /// <summary>
    /// Default constructor
    /// </summary>
    public ConfiguratorViewModel(NavigationManager navigationManager)
    {
      m_navigationManager = navigationManager;
      BackCommand = ReactiveCommand.Create(NavigateBack);
    }

    #region Methods

    private void NavigateBack()
      => m_navigationManager.NavigateTo(PageNames.HOME);

    /// <inheritdoc />
    public override void SetNamedArguments(IReadOnlyDictionary<string, string> arguments)
    {
      m_plugin = PluginManager.GetPlugin(arguments.First().Key);
      this.RaisePropertyChanged(nameof(Title));
    }

    #endregion
  }
}