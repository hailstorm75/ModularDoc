using System;
using System.Collections.Generic;
using MarkDoc.Core;

namespace MarkDoc.MVVM.Helpers
{
  /// <summary>
  /// Base view model for plugin steps
  /// </summary>
  /// <typeparam name="TSettings">Step settings type</typeparam>
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

    #endregion

    /// <inheritdoc />
    public abstract void SetNamedArguments(IReadOnlyDictionary<string, string> arguments);

    protected void OnCanProceed(bool canProceed)
      => CanProceed?.Invoke(this, canProceed);
  }
}