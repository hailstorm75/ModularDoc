using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Runtime.Loader;
using MarkDoc.Helpers;
using MarkDoc.Members;

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
      lock (this)
      {
        if (m_composed)
          return;

        var path = Path.GetFullPath("../../../Components/Members");
        var assemblies = Directory
          .GetFiles(path, "*.dll", SearchOption.TopDirectoryOnly)
          .Concat(Directory.GetFiles(Path.GetFullPath("../../../"), "dnlib.dll", SearchOption.TopDirectoryOnly))
          .Select(AssemblyLoadContext.Default.LoadFromAssemblyPath)
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
