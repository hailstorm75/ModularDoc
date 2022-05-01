using System.Collections.Generic;
using System.Windows.Input;
using MarkDoc.Core;

namespace MarkDoc.ViewModels
{
  /// <summary>
  /// Interface for home view models
  /// </summary>
  public interface IHomeViewModel
    : IViewModel
  {
    #region Properties

    /// <summary>
    /// Collection of available plugins
    /// </summary>
    IReadOnlyCollection<IPlugin> Plugins { get; }

    /// <summary>
    /// Currently selected plugin
    /// </summary>
    IPlugin? SelectedPlugin { get; set; }

    /// <summary>
    /// Determines whether the pane is open
    /// </summary>
    bool PaneOpen { get; set; }

    /// <summary>
    /// Search term for filtering plugins
    /// </summary>
    string SearchTerm { get; set; }

    #endregion

    #region Commands

    /// <summary>
    /// Command for opening a plugin with new settings
    /// </summary>
    ICommand PluginNewCommand { get; }

    /// <summary>
    /// Command for closing the selected plugin
    /// </summary>
    ICommand PluginCancelCommand { get; }

    /// <summary>
    /// Command for opening a plugin with existing settings
    /// </summary>
    ICommand PluginOpenCommand { get; }

    /// <summary>
    /// Opens application settings
    /// </summary>
    ICommand OpenSettingsCommand { get; }

    /// <summary>
    /// Clears the currently entered search term
    /// </summary>
    ICommand ClearSearchCommand { get; }

    #endregion
  }
}