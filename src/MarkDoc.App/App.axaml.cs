using Autofac;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using MarkDoc.App.Views;
using MarkDoc.Constants;
using MarkDoc.MVVM.Helpers;
using MarkDoc.ViewModels;
using MarkDoc.ViewModels.Main;
using MarkDoc.Views;
using MarkDoc.Views.Main;

namespace MarkDoc.App
{
  public class App
    : Application
  {
    public override void Initialize()
    {
      var builder = new ContainerBuilder();
      builder.RegisterType<SettingsViewModel>().As<ISettingsViewModel>();
      builder.RegisterType<SettingsView>().As<ISettingsView>();
      builder.RegisterType<StartupViewModel>().As<IStartupViewModel>();
      builder.RegisterType<StartupView>().As<IStartupView>();
      builder.RegisterType<HomeViewModel>().As<IHomeViewModel>();
      builder.RegisterType<HomeView>().As<IHomeView>();

      var navigator = new NavigationManager();
      builder.RegisterInstance(navigator).SingleInstance();
      navigator.Register<ISettingsView>(PageNames.SETTINGS);
      navigator.Register<IStartupView>(PageNames.STARTUP);
      navigator.Register<IHomeView>(PageNames.HOME);

      var container = builder.Build();
      TypeResolver.Initialize(container);

      AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
      if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        desktop.MainWindow = new MainWindow();

      base.OnFrameworkInitializationCompleted();
    }
  }
}
