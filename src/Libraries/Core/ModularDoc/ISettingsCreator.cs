using System.Collections.Generic;

namespace ModularDoc
{
  /// <summary>
  /// Interface for settings creators
  /// </summary>
  public interface ISettingsCreator
  {
    /// <summary>
    /// Converts given <paramref name="data"/> to settings of a given type
    /// </summary>
    /// <param name="data">Data to convert to <typeparamref name="T"/></param>
    /// <typeparam name="T">Settings type</typeparam>
    /// <returns>Conversion result</returns>
    T CreateSettings<T>(IReadOnlyDictionary<string, string> data) where T : ILibrarySettings;
  }
}