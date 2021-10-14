using System;
using System.Collections.Generic;
using MarkDoc.Core;

namespace MarkDoc.MVVM.Helpers
{
  public abstract class BaseStepViewModel<TSettings>
    : IStepViewModel<TSettings>
    where TSettings : ILibrarySettings
  {
    /// <inheritdoc />
    public void SetNamedArguments(IReadOnlyDictionary<string, string> arguments)
      => throw new NotImplementedException();

    public void SetSettings(TSettings settings)
    {
    }
  }
}