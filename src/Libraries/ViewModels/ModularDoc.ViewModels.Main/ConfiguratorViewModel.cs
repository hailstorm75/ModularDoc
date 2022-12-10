using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using ModularDoc.Constants;
using ModularDoc;
using ModularDoc.Core;
using ModularDoc.Helpers;
using ModularDoc.MVVM.Helpers;
using ReactiveUI;

namespace ModularDoc.ViewModels.Main
{
  public class ConfiguratorViewModel
    : BaseViewModel, IConfiguratorViewModel
  {
    #region Fields

    private readonly NavigationManager m_navigationManager;
    private IPlugin? m_plugin;
    private IPluginStep? m_currentStep;
    private IStepView<IStepViewModel>? m_currentView;

    #endregion

    #region Properties

    /// <inheritdoc />
    public string Title => "Plugin: " + (m_plugin?.Name ?? string.Empty);

    /// <inheritdoc />
    public ObservableCollection<IPluginStep> Steps { get; } = new ();

    /// <inheritdoc />
    public IReadOnlyDictionary<string, string> CurrentStepSettings { get; private set; } = new Dictionary<string, string>();

    /// <inheritdoc />
    public IPluginStep? CurrentStep
    {
      get => m_currentStep;
      set
      {
        if (m_currentStep == value)
          return;

        m_currentStep = value;
        this.RaisePropertyChanged(nameof(CurrentStep));

        if (value is null)
          return;

        // ReSharper disable once AssignNullToNotNullAttribute
        CurrentStepSettings = PluginSettings.TryGetValue(value.Id, out var settings)
          ? settings
          : new Dictionary<string, string>(0);

        var previousSettings = Steps
          .Take(Steps.IndexOf(value))
          .Select(plugin =>
          {
            var pluginSettings = PluginSettings.TryGetValue(plugin.Id, out var result)
              ? result
              : new Dictionary<string, string>(0);

            return new KeyValuePair<string, IReadOnlyDictionary<string, string>>(plugin.GetViewId(), pluginSettings);
          })
          .ToDictionary(x => x.Key, x => x.Value);

        // ReSharper disable once AssignNullToNotNullAttribute
        CurrentView = value.GetStepViewAsync(CurrentStepSettings, previousSettings).Result;
      }
    }

    /// <inheritdoc />
    public IStepView<IStepViewModel>? CurrentView
    {
      get => m_currentView;
      set
      {
        m_currentView = value;
        this.RaisePropertyChanged(nameof(CurrentView));
      }
    }

    /// <inheritdoc />
    public Dictionary<string, IReadOnlyDictionary<string, string>> PluginSettings { get; private set; } = new();

    #endregion

    #region Commands

    /// <inheritdoc />
    public ICommand BackCommand { get; }

    /// <inheritdoc />
    public ICommand PreviousStageCommand { get; }

    /// <inheritdoc />
    public ICommand NextStageCommand { get; }

    /// <inheritdoc />
    public ICommand FinishCommand { get; }

    #endregion

    /// <summary>
    /// Default constructor
    /// </summary>
    public ConfiguratorViewModel(NavigationManager navigationManager)
    {
      m_navigationManager = navigationManager;

      var canNavigateToNext = this
        .WhenAnyValue(viewModel => viewModel.CurrentView!.ViewModel.IsValid);

      BackCommand = ReactiveCommand.Create(NavigateBack);
      FinishCommand = ReactiveCommand.Create(NavigateSummary, canNavigateToNext);

      NextStageCommand = ReactiveCommand.Create(NextStep, canNavigateToNext);
      PreviousStageCommand = ReactiveCommand.Create(PreviousStep);
    }

    #region Methods

    private void NavigateBack()
      => m_navigationManager.NavigateTo(PageNames.HOME);

    private async Task NavigateSummary()
    {
      SaveStepSettings();

      await using var stream = new MemoryStream();
      await JsonSerializer.SerializeAsync(stream, PluginSettings);

      stream.Position = 0;
      using var reader = new StreamReader(stream);
      var settings = await reader.ReadToEndAsync();

      m_navigationManager.NavigateTo(PageNames.SUMMARY, new Dictionary<string, string>
      {
        { IConfiguratorViewModel.ARGUMENT_ID, m_plugin!.Id },
        { IConfiguratorViewModel.ARGUMENT_SETTINGS, settings }
      });
    }

    private void SaveStepSettings()
    {
      var viewSettings = CurrentView!.ViewModel.GetSettings();
      if (!PluginSettings.TryAdd(CurrentStep!.Id, CurrentView.ViewModel.GetSettings()))
        PluginSettings[CurrentStep.Id] = viewSettings;
    }

    private void PreviousStep()
    {
      if (CurrentStep is null)
        return;

      SaveStepSettings();

      CurrentStep = Steps[Steps.IndexOf(CurrentStep) - 1];
    }

    private void NextStep()
    {
      if (CurrentStep is null || CurrentView is null)
        return;

      SaveStepSettings();

      CurrentStep = Steps[Steps.IndexOf(CurrentStep) + 1];
    }

    /// <inheritdoc />
    public override async Task SetNamedArguments(IReadOnlyDictionary<string, string> arguments)
    {
      var (id, settings, settingsPath) = ExtractArguments(arguments);

      if (!string.IsNullOrEmpty(settingsPath))
      {
        var configuration = await Configuration.LoadFromFileAsync(settingsPath).ConfigureAwait(true);
        PluginSettings = configuration.GetEditableSettings();
        id = configuration.PluginId;
      }
      else if (!string.IsNullOrEmpty(settings))
      {
        var deserialized = JsonSerializer.Deserialize<Dictionary<string, IReadOnlyDictionary<string, string>>>(settings);
        if (deserialized is not null)
          PluginSettings = deserialized;
      }

      m_plugin = PluginManager.GetPlugin(id);

      foreach (var step in m_plugin.GetPluginSteps())
      {
        Steps.Add(step);
        // If no settings were deserialized
        if (PluginSettings.Count == 0)
          PluginSettings.Add(step.Id, new Dictionary<string, string>());
      }

      CurrentStep = Steps.First();

      this.RaisePropertyChanged(nameof(Steps));
      this.RaisePropertyChanged(nameof(Title));
    }

    private static (string id, string settings, string settingsPath) ExtractArguments(IReadOnlyDictionary<string, string> arguments)
    {
      if (arguments.Count == 0)
        return ("", "", "");

      var result = ("", "", "");
      foreach (var (key, value) in arguments)
        switch (key)
        {
          case "0":
          case IConfiguratorViewModel.ARGUMENT_ID:
            result.Item1 = value;
            break;
          case IConfiguratorViewModel.ARGUMENT_SETTINGS:
            result.Item2 = value;
            break;
          case IConfiguratorViewModel.ARGUMENT_SETTINGS_PATH:
            result.Item3 = value;
            break;
        }

      return result;
    }

    #endregion
  }
}