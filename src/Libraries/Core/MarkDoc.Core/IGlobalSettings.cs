using System.Collections.Generic;

namespace MarkDoc.Core
{
  public interface IGlobalSettings
  {
    /// <summary>
    /// Globally ignored namespaces
    /// </summary>
    IReadOnlyCollection<string> IgnoredNamespaces { get; set; }

    /// <summary>
    /// Globally ignored types
    /// </summary>
    IReadOnlyCollection<string> IgnoredTypes { get; set; }
  }
}