using System.Collections.Generic;
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
    private bool m_isValid;

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

    #endregion

    /// <inheritdoc />
    public abstract void SetNamedArguments(IReadOnlyDictionary<string, string> arguments);

    public virtual void SetPreviousSettings(IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> settings)
    {
    }

    /// <inheritdoc />
    public abstract IReadOnlyDictionary<string, string> GetSettings();
  }
}