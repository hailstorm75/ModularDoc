using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Core;
using MarkDoc.Core;
using MarkDoc.Helpers;

namespace MarkDoc.MVVM.Helpers
{
  public static class PluginManager
  {
    public static IReadOnlyDictionary<string, IPlugin> Plugins { get; }

    /// <summary>
    /// Default constructor
    /// </summary>
    static PluginManager()
    {
      var builder = new ContainerBuilder();

      RegisterModules(builder);

      var container = builder.Build();
      Plugins = container.Resolve<IEnumerable<IPlugin>>().ToDictionary(plugin => plugin.Id, Linq.XtoX, StringComparer.InvariantCultureIgnoreCase);
    }

    private static void RegisterModules(ContainerBuilder builder)
    {
      var path = Path.GetFullPath("Plugins");
      var assemblies = Directory
        .EnumerateFiles(path, "MarkDoc.Plugin.*.dll", SearchOption.TopDirectoryOnly)
        .Select(Assembly.LoadFrom);

      foreach (var assembly in assemblies)
      {
        var modules = assembly.GetTypes()
          .Where(p => typeof(IModule).IsAssignableFrom(p) && !p.IsAbstract)
          .Select(Activator.CreateInstance)
          .OfType<IModule>();

        // Registers each module
        foreach (var module in modules)
          builder.RegisterModule(module);
      }
    }

    public static IPlugin GetPlugin(string id)
    {
      if (Plugins.TryGetValue(id, out var plugin))
        return plugin;

      throw new KeyNotFoundException($"Plugin with the Id '{id}' not found.");
    }
  }
}