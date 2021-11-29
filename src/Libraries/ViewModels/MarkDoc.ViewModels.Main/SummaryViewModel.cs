using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reactive.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using MarkDoc.Constants;
using MarkDoc.MVVM.Helpers;
using ReactiveUI;

namespace MarkDoc.ViewModels.Main
{
  public class SummaryViewModel
    : BaseViewModel, ISummaryViewModel
  {
    private readonly NavigationManager m_navigationManager;
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

    public ICommand ExecuteCommand { get; }

    #endregion

    /// <summary>
    /// Default constructor
    /// </summary>
    public SummaryViewModel(NavigationManager navigationManager)
    {
      m_navigationManager = navigationManager;

      var canExecute = this.WhenAnyValue(viewModel => viewModel.Loading).Select(x => !x);

      BackCommand = ReactiveCommand.Create(NavigateBack);
      ExecuteCommand = ReactiveCommand.CreateFromTask(ExecuteAsync, canExecute);
    }

    /// <inheritdoc />
    [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
    public async Task ExecuteAsync()
    {
      Loading = true;

      if (!m_pluginSettings.TryGetValue(IConfiguratorViewModel.ARGUMENT_ID, out var pluginId) || !m_pluginSettings.TryGetValue(IConfiguratorViewModel.ARGUMENT_SETTINGS, out var settings))
        return;

      // ReSharper disable once AssignNullToNotNullAttribute
      var deserialized = JsonSerializer.Deserialize<Dictionary<string, IReadOnlyDictionary<string, string>>>(settings);
      // ReSharper disable once AssignNullToNotNullAttribute
      var plugin = PluginManager.GetPlugin(pluginId);
      await plugin.ExecuteAsync(deserialized ?? new Dictionary<string, IReadOnlyDictionary<string, string>>());

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