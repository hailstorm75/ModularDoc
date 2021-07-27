using System.Collections.Generic;
using System.Windows.Input;
using MarkDoc.Core;
using MarkDoc.MVVM.Helpers;

namespace MarkDoc.ViewModels.Main
{
  public class HomeViewModel
    : IHomeViewModel
  {
    /// <inheritdoc />
    public IReadOnlyCollection<IPlugin> Plugins
      => PluginManagers.Plugins;

    /// <inheritdoc />
    public ICommand PluginSelectedCommand { get; }
  }
}