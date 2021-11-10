using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    private IPluginStep? m_currentStep;
    private IView? m_currentView;

    #endregion

    #region Properties

    /// <inheritdoc />
    public string Title => "Plugin: " + (m_plugin?.Name ?? string.Empty);

    public ObservableCollection<IPluginStep> Steps { get; } = new ();

    /// <inheritdoc />
    public IPluginStep? CurrentStep
    {
      get => m_currentStep;
      set
      {
        if (m_currentStep == value)
          return;

        m_currentStep = value;

        if (value is not null)
          CurrentView = value.GetStepView();

        this.RaisePropertyChanging(nameof(CurrentStep));
      }
    }

    /// <inheritdoc />
    public IView? CurrentView
    {
      get => m_currentView;
      private set
      {
        m_currentView = value;
        this.RaisePropertyChanging(nameof(CurrentView));
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
      var id = arguments.FirstOrDefault().Key;

      m_plugin = PluginManager.GetPlugin(id);

      foreach (var view in m_plugin.GetPluginSteps())
        Steps.Add(view);

      CurrentStep = Steps.First();

      this.RaisePropertyChanged(nameof(Steps));
      this.RaisePropertyChanged(nameof(Title));
    }

    #endregion
  }
}