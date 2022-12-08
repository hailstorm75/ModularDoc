using Autofac;
using ModularDoc;
using ModularDoc.Helpers;
using ModularDoc.MVVM.Helpers;

var configurationPath = args.FirstOrDefault();
if (string.IsNullOrEmpty(configurationPath))
{
  Console.WriteLine("No configuration file specified");
  return;
}

var builder = new ContainerBuilder();
builder.RegisterType<Logger>().As<IModularDocLogger>();
PluginManager.RegisterModules(builder);

var container = builder.Build();
TypeResolver.Initialize(container);

var configuration = await Configuration.LoadFromFileAsync(configurationPath).ConfigureAwait(true);
var plugin = PluginManager.GetPlugin(configuration.PluginId);
var (logger, _, executor) = plugin.GenerateExecutor(configuration.Settings);

void Log(object? sender, LogMessage e) => Console.WriteLine(@$"[{TimeOnly.FromDateTime(e.Time)}] [{e.Source}] [{e.Type}] {e.Message}");

logger.NewLog += Log;

await executor(CancellationToken.None);
