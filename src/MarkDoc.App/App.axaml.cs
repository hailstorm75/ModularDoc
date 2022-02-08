using Autofac;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using MarkDoc.App.Managers;
using MarkDoc.App.Views;
using MarkDoc.Constants;
using MarkDoc.Helpers;
using MarkDoc.MVVM.Helpers;
using MarkDoc.ViewModels;
using MarkDoc.ViewModels.Main;
using MarkDoc.Views;
using MarkDoc.Views.Main;
using System;
using MarkDoc.Core;

namespace MarkDoc.App
{
  public sealed class App
    : Application
  {
    private readonly DialogManager m_dialogManager;

    public App()
    {
      Window WindowProvider()
      {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
          return desktop.MainWindow;

        throw new Exception();
      }

      m_dialogManager = new DialogManager(WindowProvider);
    }

    /// <inheritdoc />
    public override void Initialize()
    {
      var builder = new ContainerBuilder();
      builder.RegisterType<ConfiguratorViewModel>().As<IConfiguratorViewModel>();
      builder.RegisterType<ConfiguratorView>().As<IConfiguratorView>();
      builder.RegisterType<SummaryViewModel>().As<ISummaryViewModel>();
      builder.RegisterType<SummaryView>().As<ISummaryView>();
      builder.RegisterType<SettingsViewModel>().As<ISettingsViewModel>();
      builder.RegisterType<SettingsView>().As<ISettingsView>();
      builder.RegisterType<StartupViewModel>().As<IStartupViewModel>();
      builder.RegisterType<StartupView>().As<IStartupView>();
      builder.RegisterType<HomeViewModel>().As<IHomeViewModel>();
      builder.RegisterType<HomeView>().As<IHomeView>();

      builder.RegisterType<Logger>().As<IMarkDocLogger>();

      builder.RegisterInstance(m_dialogManager).As<IDialogManager>().SingleInstance();

      PluginManager.RegisterModules(builder);

      var navigator = new NavigationManager();
      builder.RegisterInstance(navigator).SingleInstance();
      navigator.Register<IConfiguratorView>(PageNames.CONFIGURATION);
      navigator.Register<ISettingsView>(PageNames.SETTINGS);
      navigator.Register<ISummaryView>(PageNames.SUMMARY);
      navigator.Register<IStartupView>(PageNames.STARTUP);
      navigator.Register<IHomeView>(PageNames.HOME);

      var container = builder.Build();
      TypeResolver.Initialize(container);

      AvaloniaXamlLoader.Load(this);
    }

    /// <inheritdoc />
    public override void OnFrameworkInitializationCompleted()
    {
      if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        desktop.MainWindow = new MainWindow();

      base.OnFrameworkInitializationCompleted();
    }
  }
}
