using System.Collections.Generic;
using System.IO;
using System.Threading;
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

    /// <summary>
    /// Retrieves settings based on the provided <paramref name="data"/>
    /// </summary>
    /// <param name="data">Data to convert</param>
    /// <typeparam name="T">Settings type</typeparam>
    /// <returns>Conversion result</returns>
    T GetSettings<T>(IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> data) where T : ILibrarySettings;

    /// <summary>
    /// Executes the plugin operation
    /// </summary>
    /// <param name="data">Settings for the plugin execution</param>
    /// <param name="cancellationToken">Operation cancellation token</param>
    Task ExecuteAsync(IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> data, CancellationToken cancellationToken);
  }
}