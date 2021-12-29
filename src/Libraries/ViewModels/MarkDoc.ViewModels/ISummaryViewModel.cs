using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
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
    /// <summary>
    /// View title
    /// </summary>
    string Title { get; }

    /// <summary>
    /// Execution logs
    /// </summary>
    ConcurrentBag<LogMessage> LogMessages { get; }

    /// <summary>
    /// Command for navigating back
    /// </summary>
    ICommand BackCommand { get; }

    /// <summary>
    /// Command for cancelling the operation
    /// </summary>
    ICommand CancelCommand { get; }
  }
}