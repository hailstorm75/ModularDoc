using System.Collections.Generic;
using System.IO;

namespace MarkDoc.Core
{
  public interface IPlugin
  {
    #region Properties

    string Id { get; }

    string Name { get; }

    string Description { get; }

    Stream? Image { get; }

    #endregion

    IReadOnlyCollection<IPluginStep> GetPluginSteps();
  }
}