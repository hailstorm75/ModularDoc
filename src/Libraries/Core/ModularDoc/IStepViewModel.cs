using System.Collections.Generic;
using System.Threading.Tasks;

namespace ModularDoc
{
  /// <summary>
  /// Interface for view models of plugin steps
  /// </summary>
  /// <typeparam name="TSettings">Step settings type</typeparam>
  // ReSharper disable once UnusedTypeParameter
  public interface IStepViewModel<TSettings>
    : IStepViewModel
    where TSettings : ILibrarySettings
  {
  }

  /// <summary>
  /// Interface for view models of plugin steps
  /// </summary>
  public interface IStepViewModel
    : IViewModel
  {
    /// <summary>
    /// Determines whether the step form is filled correctly
    /// </summary>
    bool IsValid { get; }

    /// <summary>
    /// Step view model Id
    /// </summary>
    /// <remarks>
    /// This Id is required for referencing previous settings
    /// </remarks>
    string Id { get; }

    /// <summary>
    /// Step name
    /// </summary>
    string Title { get; }

    /// <summary>
    /// Step description
    /// </summary>
    string Description { get; }

    /// <summary>
    /// Retrieves current settings
    /// </summary>
    /// <returns>Current form settings</returns>
    IReadOnlyDictionary<string, string> GetSettings();

    /// <summary>
    /// Sets previous step settings
    /// </summary>
    /// <param name="settings">Previous settings</param>
    ValueTask SetPreviousSettings(IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> settings);
  }
}