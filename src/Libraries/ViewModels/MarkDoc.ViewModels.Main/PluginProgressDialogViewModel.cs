using System.Collections.Generic;
using System.Text.Json;
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
    private CancellationTokenSource? m_tokenSource;

    /// <inheritdoc />
    public override Task SetNamedArguments(IReadOnlyDictionary<string, string> arguments)
    {
      arguments.TryGetValue("settings", out var settings);
      m_settings = settings ?? string.Empty;

      arguments.TryGetValue("id", out var pluginId);
      m_pluginId = pluginId ?? string.Empty;

      return Task.CompletedTask;
    }

    public async ValueTask Execute()
    {
      m_tokenSource = new CancellationTokenSource();

      // ReSharper disable once AssignNullToNotNullAttribute
      var deserialized = JsonSerializer.Deserialize<Dictionary<string, IReadOnlyDictionary<string, string>>>(m_settings);
      // ReSharper disable once AssignNullToNotNullAttribute
      var plugin = PluginManager.GetPlugin(m_pluginId);

      Title = plugin.Name;

      await plugin.ExecuteAsync(deserialized ?? new Dictionary<string, IReadOnlyDictionary<string, string>>(), m_tokenSource.Token);

      if (m_tokenSource.IsCancellationRequested)
        return;

      CanClickPositive = true;
      CanClickCancel = false;
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