using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MarkDoc.Core;
using ReactiveUI;

namespace MarkDoc.MVVM.Helpers
{
  public abstract class BaseViewModel
    : ReactiveObject, IViewModel
  {
    private bool m_isLoading;

    #region Properties

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

    // ReSharper disable once AnnotateCanBeNullTypeMember
    protected T ExecuteLoading<T>(Func<T> action)
    {
      IsLoading = true;
      var result = action();
      IsLoading = false;

      return result;
    }

    /// <inheritdoc />
    public virtual Task SetNamedArguments(IReadOnlyDictionary<string, string> arguments) => Task.CompletedTask;

    /// <inheritdoc />
    public virtual ValueTask OnLoadedAsync() => ValueTask.CompletedTask;

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