using System;

namespace MarkDoc.Core
{
  public interface IStepViewModel<TSettings>
    : IViewModel
    where TSettings : ILibrarySettings
  {
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

    void SetSettings(TSettings settings);

    TSettings GetSettings();
  }
}