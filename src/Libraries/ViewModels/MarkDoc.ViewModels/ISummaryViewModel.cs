﻿using System.Collections.Generic;
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

    #endregion
  }
}