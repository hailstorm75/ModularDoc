using System.Collections.Generic;
using System.Linq;
using MarkDoc.Core;
using MarkDoc.MVVM.Helpers;
using ReactiveUI;

namespace MarkDoc.ViewModels.Main
{
  public class ConfiguratorViewModel
    : BaseViewModel, IConfiguratorViewModel
  {
    private IPlugin? m_plugin;

    /// <inheritdoc />
    public string Title => "Plugin: " + (m_plugin?.Name ?? string.Empty);

    /// <inheritdoc />
    public override void SetArguments(IReadOnlyCollection<string> arguments)
    {
      m_plugin = PluginManager.GetPlugin(arguments.First());
      this.RaisePropertyChanged(nameof(Title));
    }
  }
}