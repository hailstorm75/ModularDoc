using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ModularDoc
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
    /// Plugin author
    /// </summary>
    string Author { get; }

    /// <summary>
    /// Plugin image
    /// </summary>
    Stream? Image { get; }

    /// <summary>
    /// List of plugin step names
    /// </summary>
    IReadOnlyCollection<string> Steps { get; }

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
    /// Creates the plugin process executor along with complementary loggers
    /// </summary>
    /// <param name="configuration">Plugin executor configuration</param>
    /// <returns>A logger of component execution details, individual component process progress loggers, executor instance</returns>
    (IModularDocLogger logger, IReadOnlyCollection<IProcess> processes, Func<CancellationToken, ValueTask> executor) GenerateExecutor(IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> configuration);
  }
}