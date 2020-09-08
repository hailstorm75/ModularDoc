using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using MarkDoc.Helpers;
using MarkDoc.Members;
using Microsoft.Extensions.DependencyModel;

namespace UT.Members.Data
{
  public class ResolversProvider
    : IEnumerable<object[]>
  {
    private bool m_composed;

    [ImportMany(AllowRecomposition = true, RequiredCreationPolicy = CreationPolicy.NonShared)]
    public IReadOnlyCollection<object[]> Resolvers { get; private set; } = new object[][] { };

    private void Compose()
    {
      static Assembly Load(string assemblyFullPath)
      {
        var fileNameWithOutExtension = Path.GetFileNameWithoutExtension(assemblyFullPath);

        var inCompileLibraries = DependencyContext.Default.CompileLibraries.Any(l => l.Name.Equals(fileNameWithOutExtension, StringComparison.OrdinalIgnoreCase));
        var inRuntimeLibraries = DependencyContext.Default.RuntimeLibraries.Any(l => l.Name.Equals(fileNameWithOutExtension, StringComparison.OrdinalIgnoreCase));

        var assembly = (inCompileLibraries || inRuntimeLibraries)
          ? Assembly.Load(new AssemblyName(fileNameWithOutExtension))
          : AssemblyLoadContext.Default.LoadFromAssemblyPath(assemblyFullPath);

        return assembly;
      }

      lock (this)
      {
        if (m_composed)
          return;

        var path = Path.GetFullPath("../../../Components/Members");
        var assemblies = Directory
          .GetFiles(path, "*.dll", SearchOption.TopDirectoryOnly)
          .Select(Load)
          .ToArray();
        var configuration = new ContainerConfiguration()
          .WithAssemblies(assemblies);
        using var container = configuration.CreateContainer();
        Resolvers = container.GetExports<IResolver>().Select(resolver => new[] { resolver }).ToReadOnlyCollection();
        m_composed = true;
      }
    }

    /// <inheritdoc />
    public IEnumerator<object[]> GetEnumerator()
    {
      if (!m_composed)
        Compose();

      return Resolvers.GetEnumerator();
    }

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
  }
}
