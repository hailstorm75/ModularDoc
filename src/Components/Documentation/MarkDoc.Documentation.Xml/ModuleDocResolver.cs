using Autofac;

namespace MarkDoc.Documentation.Xml
{
  /// <summary>
  /// Class for exporting the <see cref="DocResolver"/> to Autofac IoC
  /// </summary>
  public class ModuleDocResolver
    : Module
  {
    /// <inheritdoc />
    protected override void Load(ContainerBuilder builder)
      => builder.RegisterType<DocResolver>().As<IDocResolver>();
  }
}