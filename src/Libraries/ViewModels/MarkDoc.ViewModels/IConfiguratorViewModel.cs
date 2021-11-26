using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using MarkDoc.Core;

namespace MarkDoc.ViewModels
{
  /// <summary>
  /// Interface for configurator view models
  /// </summary>
  public interface IConfiguratorViewModel
    : IViewModel, ICanGoBack
  {
    #region Fields

    public const string ARGUMENT_ID = "id";
    public const string ARGUMENT_SETTINGS = "settings";

    #endregion

    #region Properties

    /// <summary>
    /// Plugin title
    /// </summary>
    string Title { get; }

    /// <summary>
    /// Steps available for configuring
    /// </summary>
    ObservableCollection<IPluginStep> Steps { get; }

    /// <summary>
    /// Currently active step
    /// </summary>
    IPluginStep? CurrentStep { get; }

    /// <summary>
    /// Currently displayed view
    /// </summary>
    IStepView<IStepViewModel>? CurrentView { get; set; }

    /// <summary>
    /// Currently active step settings
    /// </summary>
    IReadOnlyDictionary<string, string> CurrentStepSettings { get; }

    /// <summary>
    /// All plugin step settings
    /// </summary>
    Dictionary<string, IReadOnlyDictionary<string, string>> PluginSettings { get; }

    #endregion

    #region Commands

    /// <summary>
    /// Command for preceding to the previous step
    /// </summary>
    ICommand PreviousStageCommand { get; }

    /// <summary>
    /// Command for proceeding to the next step
    /// </summary>
    ICommand NextStageCommand { get; }

    /// <summary>
    /// Command for proceeding to the summary
    /// </summary>
    ICommand FinishCommand { get; }

    #endregion
  }
}