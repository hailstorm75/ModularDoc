using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Core;
using MarkDoc.Members;

namespace UT.Members.Data
{
  public class ResolversProvider
    : IEnumerable<IResolver>
  {
    private static readonly IContainer CONTAINER;

    static ResolversProvider()
    {
      var builder = new ContainerBuilder();

      RegisterModules(builder);

      CONTAINER = builder.Build();
    }

    private static void RegisterModules(ContainerBuilder builder)
    {
      var path = Path.GetFullPath("../../../Components/Members");
      var assemblies = Directory
        .EnumerateFiles(path, "MarkDoc*.dll", SearchOption.TopDirectoryOnly)
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

    /// <inheritdoc />
    public IEnumerator<IResolver> GetEnumerator() => CONTAINER.Resolve<IEnumerable<IResolver>>().GetEnumerator();

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
  }
}