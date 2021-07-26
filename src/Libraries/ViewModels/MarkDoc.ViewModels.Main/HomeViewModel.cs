using System.Collections.Generic;
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
  }
}