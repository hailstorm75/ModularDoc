using System;
using System.Collections.Generic;
using System.Linq;
using Autofac.Extras.Moq;
using MarkDoc.Core;

namespace MarkDoc.MVVM.Helpers
{
  public static class PluginManager
  {
    public static IReadOnlyCollection<IPlugin> Plugins { get; }

    /// <summary>
    /// Default constructor
    /// </summary>
    static PluginManager()
    {
      var plugins = new LinkedList<IPlugin>();

      for (var i = 1; i < 5; i++)
      {
        using var mock = AutoMock.GetLoose();
        var plugin = mock.Mock<IPlugin>();
        plugin.SetupGet(x => x.Id).Returns(Guid.NewGuid().ToString());
        plugin.SetupGet(x => x.Description).Returns("Description");
        plugin.SetupGet(x => x.Name).Returns("Name " + i);

        plugins.AddLast(plugin.Object);
      }

      Plugins = plugins;
    }

    public static IPlugin? GetPlugin(string id)
      => Plugins.FirstOrDefault(x => x.Id.Equals(id, StringComparison.InvariantCultureIgnoreCase));
  }
}