using System;

namespace MarkDoc.Core
{
  /// <summary>
  /// Interface for view models of plugin steps
  /// </summary>
  /// <typeparam name="TSettings">Step settings type</typeparam>
  public interface IStepViewModel<TSettings>
    : IViewModel
    where TSettings : ILibrarySettings
  {
    /// <summary>
    /// Invoked when if the ability to proceed to the next step changes
    /// </summary>
    event EventHandler<bool>? CanProceed;

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
}