using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Core;
using MarkDoc.Documentation;
using MarkDoc.Members;
using Moq;

namespace UT.Documentation.Data
{
  internal class ResolversProvider
    : IEnumerable<IDocResolver>
  {
    private static readonly IContainer CONTAINER;

    static ResolversProvider()
    {
      var builder = new ContainerBuilder();

      RegisterModules(builder);
      MockResolver(builder);

      CONTAINER = builder.Build();
    }

    private static void RegisterModules(ContainerBuilder builder)
    {
      var path = Path.GetFullPath("../../../Components/Documentation");
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

    private static void MockResolver(ContainerBuilder builder)
    {
      var resolver = new Mock<IResolver>().Object;

      builder.RegisterInstance(resolver).As<IResolver>();
    }

    /// <inheritdoc />
    public IEnumerator<IDocResolver> GetEnumerator() => CONTAINER.Resolve<IEnumerable<IDocResolver>>().GetEnumerator();

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
  }
}
