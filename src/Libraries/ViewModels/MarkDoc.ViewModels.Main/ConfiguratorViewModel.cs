using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json;
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
    private IPluginStep? m_currentStep;

    #endregion

    #region Properties

    /// <inheritdoc />
    public string Title => "Plugin: " + (m_plugin?.Name ?? string.Empty);

    /// <inheritdoc />
    public ObservableCollection<IPluginStep> Steps { get; } = new ();

    /// <inheritdoc />
    public Dictionary<string, string> CurrentStepSettings { get; private set; } = new();

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
          : new Dictionary<string, string>();
      }
    }

    /// <inheritdoc />
    public Dictionary<string, Dictionary<string, string>> PluginSettings { get; private set; } = new();

    #endregion

    #region Commands

    /// <inheritdoc />
    public ICommand BackCommand { get; }

    // public ICommand NextStageCommand { get; }

    // public ICommand FinishCommand { get; }

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
      var (id, settings) = ExtractArguments(arguments);

      m_plugin = PluginManager.GetPlugin(id);

      if (!string.IsNullOrEmpty(settings))
      {
        var deserialized = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, string>>>(settings);
        if (deserialized is not null)
          PluginSettings = deserialized;
      }

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

    private static (string id, string settings) ExtractArguments(IReadOnlyDictionary<string, string> arguments)
    {
      if (arguments.Count == 0)
        return ("", "");

      var result = ("", "");
      foreach (var (key, value) in arguments)
        switch (key.ToLowerInvariant())
        {
          case "0":
          case IConfiguratorViewModel.ARGUMENT_ID:
            result.Item1 = value;
            break;
          case IConfiguratorViewModel.ARGUMENT_SETTINGS:
            result.Item2 = value;
            break;
        }

      return result;
    }

    #endregion
  }
}