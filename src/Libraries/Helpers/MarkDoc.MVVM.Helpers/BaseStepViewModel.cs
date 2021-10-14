using System.Collections.Generic;
using MarkDoc.Core;

namespace MarkDoc.MVVM.Helpers
{
  public abstract class BaseStepViewModel<TSettings>
    : IStepViewModel<TSettings>
    where TSettings : ILibrarySettings
  {
    public void SetNamedArguments(IReadOnlyDictionary<string, string> arguments)
    {

    }

    public void SetSettings(TSettings settings)
    {
    }
  }
}