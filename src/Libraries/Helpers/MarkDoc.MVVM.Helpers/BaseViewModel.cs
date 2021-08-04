using System.Collections.Generic;
using MarkDoc.ViewModels;
using ReactiveUI;

namespace MarkDoc.MVVM.Helpers
{
  public abstract class BaseViewModel
    : ReactiveObject, IViewModel
  {
    /// <inheritdoc />
    public virtual void SetArguments(IReadOnlyCollection<string> arguments) {}

    /// <inheritdoc />
    public virtual void SetNamedArguments(IReadOnlyDictionary<string, string> arguments) {}
  }
}