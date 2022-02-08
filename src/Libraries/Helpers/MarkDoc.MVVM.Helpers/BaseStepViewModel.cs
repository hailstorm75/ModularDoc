using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MarkDoc.Core;
using ReactiveUI;

namespace MarkDoc.MVVM.Helpers
{
  /// <summary>
  /// Base view model for plugin steps
  /// </summary>
  /// <typeparam name="TSettings">Step settings type</typeparam>
  public abstract class BaseStepViewModel<TSettings>
    : ReactiveObject, IStepViewModel<TSettings>
    where TSettings : ILibrarySettings
  {
    #region Fields

    private bool m_isValid;
    private bool m_isLoading;

    #endregion

    #region Properties

    public abstract string Id { get; }

    /// <inheritdoc />
    public abstract string Title { get; }

    /// <inheritdoc />
    public abstract string Description { get; }

    /// <inheritdoc />
    public bool IsValid
    {
      get => m_isValid;
      protected set
      {
        m_isValid = value;
        this.RaisePropertyChanged(nameof(IsValid));
      }
    }

    /// <inheritdoc />
    public bool IsLoading
    {
      get => m_isLoading;
      protected set
      {
        m_isLoading = value;
        this.RaisePropertyChanged(nameof(IsLoading));
      }
    }

    #endregion

    #region Methods

    /// <inheritdoc />
    public abstract Task SetNamedArguments(IReadOnlyDictionary<string, string> arguments);

    /// <inheritdoc />
    public virtual ValueTask OnLoadedAsync() => ValueTask.CompletedTask;

    public virtual ValueTask SetPreviousSettings(
      IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> settings)
      => ValueTask.CompletedTask;

    /// <inheritdoc />
    public abstract IReadOnlyDictionary<string, string> GetSettings();

    protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
      => this.RaisePropertyChanged(propertyName);

    protected virtual void Dispose(bool disposing) {}

    /// <inheritdoc />
    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    #endregion
  }
}