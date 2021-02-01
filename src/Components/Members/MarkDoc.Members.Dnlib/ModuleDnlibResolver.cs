using Autofac;

namespace MarkDoc.Members.Dnlib
{
  /// <summary>
  /// Class for exporting the <see cref="Resolver"/> to Autofac IoC
  /// </summary>
  public class ModuleDnlibResolver
    : Module
  {
    /// <inheritdoc />
    protected override void Load(ContainerBuilder builder)
      => builder.RegisterType<Resolver>().As<IResolver>();
  }
}