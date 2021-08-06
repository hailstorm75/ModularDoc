using Avalonia;
using Avalonia.ReactiveUI;
using Projektanker.Icons.Avalonia;
using Projektanker.Icons.Avalonia.FontAwesome;

namespace MarkDoc.App
{
  public static class Program
  {
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    public static void Main(string[] args) => BuildAvaloniaApp()
      .StartWithClassicDesktopLifetime(args);

    // Avalonia configuration, don't remove; also used by visual designer.
    private static AppBuilder BuildAvaloniaApp()
      => AppBuilder.Configure<App>()
        .AfterSetup(AfterSetupCallback)
        .UsePlatformDetect()
        .LogToTrace()
        .UseReactiveUI();

    private static void AfterSetupCallback(AppBuilder appBuilder)
      => IconProvider.Register<FontAwesomeIconProvider>();
  }
}
