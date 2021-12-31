using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MarkDoc.Core;
using ReactiveUI;

namespace MarkDoc.MVVM.Helpers
{
  public abstract class BaseViewModel
    : ReactiveObject, IViewModel
  {
    /// <inheritdoc />
    public virtual Task SetNamedArguments(IReadOnlyDictionary<string, string> arguments) => Task.CompletedTask;

    /// <inheritdoc />
    public virtual ValueTask OnLoadedAsync() => ValueTask.CompletedTask;
  }
}