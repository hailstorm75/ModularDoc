using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace ModularDoc.Helpers
{
  public struct Configuration
  {
    /// <summary>
    /// Source plugin id
    /// </summary>
    public string PluginId { get; set; }

    /// <summary>
    /// Plugin settings
    /// </summary>
    public IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> Settings { get; set; }

    /// <summary>
    /// Default constructor
    /// </summary>
    public Configuration(string pluginId, IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> settings)
    {
      PluginId = pluginId;
      Settings = settings;
    }

    public Dictionary<string, IReadOnlyDictionary<string, string>> GetEditableSettings()
      => Settings.Select(Linq.XtoX).ToDictionary(x => x.Key, x => x.Value);

    public static async ValueTask<Configuration> LoadFromFileAsync(string path)
    {
      await using var stream = File.OpenRead(path);
      return await JsonSerializer.DeserializeAsync<Configuration>(stream).ConfigureAwait(false);
    }

    public static async ValueTask SaveToFileAsync(Configuration configuration, Stream stream)
      => await JsonSerializer.SerializeAsync(stream, configuration).ConfigureAwait(false);
  }
}