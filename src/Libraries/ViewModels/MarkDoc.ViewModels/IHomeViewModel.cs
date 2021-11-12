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
    /// <summary>
    /// Collection of available plugins
    /// </summary>
    IReadOnlyCollection<IPlugin> Plugins { get; }

    /// <summary>
    /// Command for opening a plugin with new settings
    /// </summary>
    ICommand PluginNewCommand { get; }

    /// <summary>
    /// Command for opening a plugin with existing settings
    /// </summary>
    ICommand PluginOpenCommand { get; }
  }
}