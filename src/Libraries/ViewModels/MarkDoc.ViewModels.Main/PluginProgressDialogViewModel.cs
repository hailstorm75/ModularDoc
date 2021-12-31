using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MarkDoc.MVVM.Helpers;

namespace MarkDoc.ViewModels.Main
{
  /// <summary>
  /// View model for the plugin progress dialog
  /// </summary>
  public class PluginProgressDialogViewModel
    : BaseDialogViewModel, IPluginProgressDialogViewModel
  {
    private string m_settings = string.Empty;
    private string m_pluginId = string.Empty;
    private CancellationTokenSource? m_tokenSource = new();

    /// <inheritdoc />
    public override Task SetNamedArguments(IReadOnlyDictionary<string, string> arguments)
    {
      arguments.TryGetValue("settings", out var settings);
      m_settings = settings ?? string.Empty;

      arguments.TryGetValue("id", out var pluginId);
      m_pluginId = pluginId ?? string.Empty;

      return Task.CompletedTask;
    }

    /// <inheritdoc />
    public override void OnPositiveButtonClicked()
    {
      RaiseCloseRequest();
    }

    /// <inheritdoc />
    public override void OnNegativeButtonClicked()
    {
      RaiseCloseRequest();
    }

    /// <inheritdoc />
    public override void OnCancelButtonClicked()
    {
      m_tokenSource?.Cancel();
      CanClickCancel = false;

      RaiseCloseRequest();
    }
  }
}