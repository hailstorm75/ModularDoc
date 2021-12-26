using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using MarkDoc.Constants;
using MarkDoc.Helpers;
using MarkDoc.MVVM.Helpers;
using MarkDoc.Views;
using ReactiveUI;

namespace MarkDoc.ViewModels.Main
{
  public class SummaryViewModel
    : BaseViewModel, ISummaryViewModel
  {
    private readonly NavigationManager m_navigationManager;
    private readonly IDialogManager m_dialogManager;
    private IReadOnlyDictionary<string,string> m_pluginSettings = new Dictionary<string, string>();
    private bool m_loading;

    #region Properties

    /// <inheritdoc />
    public string Title => "Summary";

    public bool Loading
    {
      get => m_loading;
      set
      {
        m_loading = value;
        this.RaisePropertyChanged();
      }
    }

    #endregion

    #region Commands

    /// <inheritdoc />
    public ICommand BackCommand { get; }

    #endregion

    /// <summary>
    /// Default constructor
    /// </summary>
    public SummaryViewModel(NavigationManager navigationManager, IDialogManager dialogManager)
    {
      m_navigationManager = navigationManager;
      m_dialogManager = dialogManager;


      BackCommand = ReactiveCommand.Create(NavigateBack);
    }

    [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
    public override async ValueTask OnLoadedAsync()
    {
      Loading = true;

      if (!m_pluginSettings.TryGetValue(IConfiguratorViewModel.ARGUMENT_ID, out var pluginId) || !m_pluginSettings.TryGetValue(IConfiguratorViewModel.ARGUMENT_SETTINGS, out var settings))
        return;

      await m_dialogManager.ShowDialogAsync<IPluginProgressDialogView>(new Dictionary<string, string>{{ "settings", settings }, { "id", pluginId }}).ConfigureAwait(false);

      Loading = false;
    }

    /// <inheritdoc />
    public override Task SetNamedArguments(IReadOnlyDictionary<string, string> arguments)
    {
      m_pluginSettings = arguments;

      return Task.CompletedTask;
    }

    private void NavigateBack()
    {
      switch (m_navigationManager.PreviousPage)
      {
        case PageNames.HOME:
          m_navigationManager.NavigateTo(PageNames.HOME);
          break;
        case PageNames.CONFIGURATION:
          m_navigationManager.NavigateTo(PageNames.CONFIGURATION, m_pluginSettings);
          break;
      }
    }
  }
}