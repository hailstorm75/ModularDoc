using System.Collections.Generic;
using MarkDoc.Core;

namespace MarkDoc.MVVM.Helpers
{
  public static class PluginManagers
  {
    public static IReadOnlyCollection<IPlugin> Plugins { get; } = null!;
  }
}