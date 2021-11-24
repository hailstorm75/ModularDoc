using System.Collections.Generic;

namespace MarkDoc.Core
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
    #region Properties

    /// <summary>
    /// Step name
    /// </summary>
    string Title { get; }

    /// <summary>
    /// Step description
    /// </summary>
    string Description { get; }

    #endregion
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
    /// Retrieves current settings
    /// </summary>
    /// <returns>Current form settings</returns>
    IReadOnlyDictionary<string, string> GetSettings();
  }
}