using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Core;
using MarkDoc.Core;
using MarkDoc.Members;
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
      MockSettings(builder);

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

    private static void MockSettings(ContainerBuilder builder)
    {
      var global = new Mock<IGlobalSettings>();
      global.SetupGet(x => x.IgnoredNamespaces)
        .Returns(Array.Empty<string>);
      global.SetupGet(x => x.IgnoredTypes)
        .Returns(Array.Empty<string>);
      var member = new Mock<IMemberSettings>();

      builder.RegisterInstance(global.Object).As<IGlobalSettings>();
      builder.RegisterInstance(member.Object).As<IMemberSettings>();
    }

    /// <inheritdoc />
    public IEnumerator<IResolver> GetEnumerator() => CONTAINER.Resolve<IEnumerable<IResolver>>().GetEnumerator();

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
  }
}