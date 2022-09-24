using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using MarkDoc.Core;

namespace MarkDoc.ViewModels
{
  /// <summary>
  /// Interface for summary view models
  /// </summary>
  public interface ISummaryViewModel
    : IViewModel
  {
    #region Properties

    /// <summary>
    /// View title
    /// </summary>
    string Title { get; }

    /// <summary>
    /// Execution logs
    /// </summary>
    ObservableCollection<LogMessage> LogMessages { get; }

    /// <summary>
    /// Plugin processes
    /// </summary>
    IReadOnlyCollection<IProcess> Processes { get; }

    /// <summary>
    /// Number of processes that are finished
    /// </summary>
    int ProcessesComplete { get; }

    /// <summary>
    /// Number of all processes
    /// </summary>
    int ProcessesTotal { get; }

    /// <summary>
    /// Determines whether logs a grouped by their source
    /// </summary>
    bool GroupLogsBySource { get; set; }

    /// <summary>
    /// Determines whether logs a grouped by their types
    /// </summary>
    bool GroupLogsByType { get; set; }

    #endregion

    #region Commands

    /// <summary>
    /// Command for navigating back
    /// </summary>
    ICommand BackCommand { get; }

    /// <summary>
    /// Command for cancelling the operation
    /// </summary>
    ICommand CancelCommand { get; }

    /// <summary>
    /// Command for returning back to the home screen
    /// </summary>
    ICommand DoneCommand { get; }

    /// <summary>
    /// Command for saving the created configuration
    /// </summary>
    ICommand SaveConfigurationCommand { get; }

    #endregion
  }
}