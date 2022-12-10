using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using Autofac;
using Autofac.Core;
using ModularDoc;
using ModularDoc.Core;
using ModularDoc.Helpers;

namespace ModularDoc.MVVM.Helpers
{
  public static class PluginManager
  {
    public static Lazy<IReadOnlyDictionary<string, IPlugin>> Plugins { get; }

    /// <summary>
    /// Default constructor
    /// </summary>
    static PluginManager()
      => Plugins = new Lazy<IReadOnlyDictionary<string, IPlugin>>(() => TypeResolver.Resolve<IEnumerable<IPlugin>>().ToDictionary(plugin => plugin.Id, Linq.XtoX, StringComparer.InvariantCultureIgnoreCase), LazyThreadSafetyMode.ExecutionAndPublication);

    public static void RegisterModules(ContainerBuilder builder)
    {
      var execPath = Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location);
      var root = Path.Combine(execPath!, "Plugins");
      var plugins = Directory.EnumerateDirectories(root);
      var assemblies = plugins
        .SelectMany(path => Directory.EnumerateFiles(path, "ModularDoc.Core.Plugins.*.dll", SearchOption.TopDirectoryOnly))
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
      if (Plugins.Value.TryGetValue(id, out var plugin))
        // ReSharper disable once AssignNullToNotNullAttribute
        return plugin;

      throw new KeyNotFoundException($"Plugin with the Id '{id}' not found.");
    }
  }
}