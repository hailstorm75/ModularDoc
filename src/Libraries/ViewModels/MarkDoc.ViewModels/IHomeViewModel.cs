using System.Collections.Generic;
using System.Windows.Input;
using MarkDoc.Core;

namespace MarkDoc.ViewModels
{
  public interface IHomeViewModel
    : IViewModel
  {
    IReadOnlyCollection<IPlugin> Plugins { get; }

    ICommand PluginSelectedCommand { get; }
  }
}