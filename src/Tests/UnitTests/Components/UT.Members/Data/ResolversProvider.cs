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
using ModularDoc.Members;
using Moq;

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
      Mock(builder);

      CONTAINER = builder.Build();
    }

    private static void RegisterModules(ContainerBuilder builder)
    {
      var path = Path.GetFullPath("../../../Components/Members");
      var assemblies = Directory
        .EnumerateFiles(path, "ModularDoc.*.dll", SearchOption.TopDirectoryOnly)
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
      var logger = new Mock<IModularDocLogger>().Object;
      var process = new Mock<IDefiniteProcess>().Object;

      builder.RegisterInstance(logger).As<IModularDocLogger>();
      builder.RegisterInstance(process).As<IDefiniteProcess>();
    }

    /// <inheritdoc />
    public IEnumerator<IResolver> GetEnumerator() => CONTAINER.Resolve<IEnumerable<IResolver>>().GetEnumerator();

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
  }
}