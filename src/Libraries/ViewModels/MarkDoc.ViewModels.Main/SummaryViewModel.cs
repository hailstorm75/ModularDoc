using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reactive.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using MarkDoc.Constants;
using MarkDoc.Core;
using MarkDoc.Helpers;
using MarkDoc.MVVM.Helpers;
using ReactiveUI;

namespace MarkDoc.ViewModels.Main
{
  public class SummaryViewModel
    : BaseViewModel, ISummaryViewModel
  {
    #region Fields

    private readonly NavigationManager m_navigationManager;
    private readonly IDialogManager m_dialogManager;
    private readonly CancellationTokenSource m_cancellationTokenSource;
    private IReadOnlyDictionary<string, string> m_pluginSettings = new Dictionary<string, string>();
    private Configuration m_pluginConfiguration;
    private bool m_loading;
    private bool m_groupLogsBySource;
    private bool m_groupLogsByType;

    #endregion

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
    public ObservableCollection<LogMessage> LogMessages { get; } = new();

    /// <inheritdoc />
    public IReadOnlyCollection<IProcess> Processes { get; private set; } = null!;

    /// <inheritdoc />
    public int ProcessesTotal { get; private set; }

    /// <inheritdoc />
    public int ProcessesComplete { get; private set; }

    /// <inheritdoc />
    public bool GroupLogsBySource
    {
      get => m_groupLogsBySource;
      set
      {
        m_groupLogsBySource = value;
        this.RaisePropertyChanged(nameof(GroupLogsBySource));
        this.RaisePropertyChanged(nameof(LogMessages));
      }
    }

    /// <inheritdoc />
    public bool GroupLogsByType
    {
      get => m_groupLogsByType;
      set
      {
        m_groupLogsByType = value;
        this.RaisePropertyChanged(nameof(GroupLogsByType));
        this.RaisePropertyChanged(nameof(LogMessages));
      }
    }

    #endregion

    #region Commands

    /// <inheritdoc />
    public ICommand BackCommand { get; }

    /// <inheritdoc />
    public ICommand CancelCommand { get; }

    /// <inheritdoc />
    public ICommand DoneCommand { get; }

    /// <inheritdoc />
    public ICommand SaveConfigurationCommand { get; }

    #endregion

    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    public SummaryViewModel(NavigationManager navigationManager, IDialogManager dialogManager)
    {
      m_cancellationTokenSource = new CancellationTokenSource();
      m_navigationManager = navigationManager;
      m_dialogManager = dialogManager;

      var canNavigateBack = this.WhenAnyValue(vm => vm.Loading).Select(x => !x);
      var canCancelOperation = this.WhenAnyValue(vm => vm.Loading);

      BackCommand = ReactiveCommand.Create(NavigateBack, canNavigateBack);
      CancelCommand = ReactiveCommand.Create(CancelOperation, canCancelOperation);
      DoneCommand = ReactiveCommand.Create(NavigateToMenu, canNavigateBack);
      SaveConfigurationCommand = ReactiveCommand.CreateFromTask(SaveConfiguration);
    }

    #endregion

    #region Methods

    [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
    public override async ValueTask OnLoadedAsync()
    {
      Loading = true;

      try
      {
        if (!m_pluginSettings.TryGetValue(IConfiguratorViewModel.ARGUMENT_ID, out var pluginId) || !m_pluginSettings.TryGetValue(IConfiguratorViewModel.ARGUMENT_SETTINGS, out var settings))
          return;

        // ReSharper disable once AssignNullToNotNullAttribute
        var deserialized = JsonSerializer.Deserialize<Dictionary<string, IReadOnlyDictionary<string, string>>>(settings);
        // ReSharper disable once AssignNullToNotNullAttribute
        var plugin = PluginManager.GetPlugin(pluginId);
        m_pluginConfiguration = new Configuration(pluginId, deserialized ?? new Dictionary<string, IReadOnlyDictionary<string, string>>());

        var (logger, processes, executor) = plugin.GenerateExecutor(m_pluginConfiguration.Settings);

        Processes = processes;
        this.RaisePropertyChanged(nameof(Processes));

        ProcessesTotal = processes.Count;
        this.RaisePropertyChanged(nameof(ProcessesTotal));

        foreach (var process in Processes)
          process.StateChanged += ProcessOnStateChanged;

        Observable.FromEventPattern<EventHandler<LogMessage>, LogMessage>(
            add => logger.NewLog += add,
            remove => logger.NewLog -= remove)
          .Select(x => x.EventArgs)
          .Buffer(TimeSpan.FromMilliseconds(100))
          .Synchronize()
          .ObserveOn(SynchronizationContext.Current!)
          .Subscribe(LogMessages.AddRange, m_cancellationTokenSource.Token);

        try
        {
          await executor(m_cancellationTokenSource.Token).ConfigureAwait(true);
        }
        finally
        {
          foreach (var process in Processes)
            process.StateChanged -= ProcessOnStateChanged;
        }
      }
      finally
      {
        Loading = false;
      }
    }

    private void ProcessOnStateChanged(object? sender, IProcess.ProcessState e)
    {
      switch (e)
      {
        case IProcess.ProcessState.Success:
        case IProcess.ProcessState.Failure:
        case IProcess.ProcessState.Cancelled:
          ProcessesComplete++;
          this.RaisePropertyChanged(nameof(ProcessesComplete));
          break;
      }
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
      => m_cancellationTokenSource.Cancel();

    private void NavigateToMenu()
      => m_navigationManager.NavigateTo(PageNames.HOME);

    private async Task SaveConfiguration()
    {
      var result = await m_dialogManager.TrySaveFileAsync("Save configuration", "pluginConfig", ("mconf", "MarkDoc config")).ConfigureAwait(false);
      if (result.IsEmpty)
        return;

      await using var fileStream = new FileStream(result.Get(), FileMode.Create);
      await Configuration.SaveToFileAsync(m_pluginConfiguration, fileStream).ConfigureAwait(false);
    }

    #endregion
  }
}