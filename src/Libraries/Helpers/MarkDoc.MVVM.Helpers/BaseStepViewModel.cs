using System;
using System.Collections.Generic;
using System.Data;
using MarkDoc.Core;

namespace MarkDoc.MVVM.Helpers
{
  public abstract class BaseStepViewModel<TSettings>
    : IStepViewModel<TSettings>
    where TSettings : ILibrarySettings
  {
    /// <inheritdoc />
    public event EventHandler<bool>? CanProceed;

    #region Properties

    /// <inheritdoc />
    public abstract string Title { get; }

    /// <inheritdoc />
    public abstract string Description { get; }

    protected TSettings? Settings { get; private set; }

    #endregion

    /// <inheritdoc />
    public void SetNamedArguments(IReadOnlyDictionary<string, string> arguments)
      => throw new NotImplementedException();

    public void SetSettings(TSettings? settings)
      => Settings = settings;

    /// <inheritdoc />
    public TSettings GetSettings()
      => Settings ?? throw new NoNullAllowedException("Settings for this view model weren't set");

    protected void OnCanProceed(bool canProceed)
      => CanProceed?.Invoke(this, canProceed);
  }
}