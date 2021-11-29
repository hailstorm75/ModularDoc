using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace MarkDoc.Core
{
  /// <summary>
  /// Interface for plugins
  /// </summary>
  public interface IPlugin
  {
    #region Properties

    /// <summary>
    /// Plugin id
    /// </summary>
    string Id { get; }

    /// <summary>
    /// Plugin name
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Plugin description
    /// </summary>
    string Description { get; }

    /// <summary>
    /// Plugin image
    /// </summary>
    Stream? Image { get; }

    #endregion

    /// <summary>
    /// Get the <see cref="IPluginStep"/> instances
    /// </summary>
    /// <returns></returns>
    IReadOnlyCollection<IPluginStep> GetPluginSteps();

    T GetSettings<T>(IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> data) where T : ILibrarySettings;

    Task ExecuteAsync(IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> data);
  }
}