using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Core;
using ModularDoc;
using ModularDoc.Core;
using ModularDoc.Documentation;
using ModularDoc.Members;
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
      Mock(builder);

      CONTAINER = builder.Build();
    }

    private static void RegisterModules(ContainerBuilder builder)
    {
      var path = Path.GetFullPath("../../../Components/Documentation");
      var assemblies = Directory
        .EnumerateFiles(path, "ModularDoc.Core*.dll", SearchOption.TopDirectoryOnly)
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

    private static void Mock(ContainerBuilder builder)
    {
      var resolver = new Mock<IResolver>().Object;
      var logger = new Mock<IModularDocLogger>().Object;
      var process = new Mock<IDefiniteProcess>().Object;
      var doc = new Mock<IDocSettings>().Object;
      var global = new Mock<IGlobalSettings>();
      global.SetupGet(x => x.IgnoredNamespaces)
        .Returns(Array.Empty<string>);
      global.SetupGet(x => x.IgnoredTypes)
        .Returns(Array.Empty<string>);
      var member = new Mock<IMemberSettings>();

      builder.RegisterInstance(global.Object).As<IGlobalSettings>();
      builder.RegisterInstance(member.Object).As<IMemberSettings>();
      builder.RegisterInstance(logger).As<IModularDocLogger>();
      builder.RegisterInstance(process).As<IDefiniteProcess>();
      builder.RegisterInstance(resolver).As<IResolver>();
      builder.RegisterInstance(doc).As<IDocSettings>();
    }

    /// <inheritdoc />
    public IEnumerator<IDocResolver> GetEnumerator() => CONTAINER.Resolve<IEnumerable<IDocResolver>>().GetEnumerator();

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
  }
}
