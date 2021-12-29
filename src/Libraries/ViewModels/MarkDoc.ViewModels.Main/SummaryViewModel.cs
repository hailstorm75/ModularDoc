using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reactive.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using MarkDoc.Constants;
using MarkDoc.Core;
using MarkDoc.MVVM.Helpers;
using ReactiveUI;

namespace MarkDoc.ViewModels.Main
{
  public class SummaryViewModel
    : BaseViewModel, ISummaryViewModel
  {
    private readonly NavigationManager m_navigationManager;
    private readonly CancellationTokenSource m_cancellationTokenSource;
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

    /// <inheritdoc />
    public ConcurrentBag<LogMessage> LogMessages { get; } = new();

    #endregion

    #region Commands

    /// <inheritdoc />
    public ICommand BackCommand { get; }

    /// <inheritdoc />
    public ICommand CancelCommand { get; }

    #endregion

    /// <summary>
    /// Default constructor
    /// </summary>
    public SummaryViewModel(NavigationManager navigationManager)
    {
      m_cancellationTokenSource = new CancellationTokenSource();
      m_navigationManager = navigationManager;

      var canNavigateBack = this.WhenAnyValue(vm => vm.Loading).Select(x => !x);
      var canCancelOperation = this.WhenAnyValue(vm => vm.Loading);

      BackCommand = ReactiveCommand.Create(NavigateBack, canNavigateBack);
      CancelCommand = ReactiveCommand.Create(CancelOperation, canCancelOperation);
    }

    [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
    public override async ValueTask OnLoadedAsync()
    {
      LogMessages.Clear();
      Loading = true;

      try
      {
        if (!m_pluginSettings.TryGetValue(IConfiguratorViewModel.ARGUMENT_ID, out var pluginId) || !m_pluginSettings.TryGetValue(IConfiguratorViewModel.ARGUMENT_SETTINGS, out var settings))
          return;

        // ReSharper disable once AssignNullToNotNullAttribute
        var deserialized = JsonSerializer.Deserialize<Dictionary<string, IReadOnlyDictionary<string, string>>>(settings);
        // ReSharper disable once AssignNullToNotNullAttribute
        var plugin = PluginManager.GetPlugin(pluginId);
        var (logger, processes, executor) = plugin.GenerateExecutor(deserialized ?? new Dictionary<string, IReadOnlyDictionary<string, string>>());
        try
        {
          logger.NewLog += LoggerOnNewLog;

          await executor(m_cancellationTokenSource.Token).ConfigureAwait(false);
        }
        finally
        {
          logger.NewLog -= LoggerOnNewLog;
        }
      }
      finally
      {
        Loading = false;
      }
    }

    private void LoggerOnNewLog(object? sender, LogMessage e)
    {
      LogMessages.Add(e);
      this.RaisePropertyChanged(nameof(LogMessages));
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

    private void CancelOperation()
    {
      m_cancellationTokenSource.Cancel();
    }
  }
}