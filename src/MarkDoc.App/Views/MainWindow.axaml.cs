using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using MarkDoc.Constants;
using MarkDoc.MVVM.Helpers;

namespace MarkDoc.App.Views
{
  public class MainWindow
    : Window
  {
    private readonly NavigationManager m_navigator;
    private TransitioningContentControl? m_mainContent;

    public MainWindow()
    {
      m_navigator = TypeResolver.Resolve<NavigationManager>();
      m_navigator.NavigationChanged += OnNavigationChanged;
      InitializeComponent();

      m_navigator.NavigateTo(PageNames.HOME);
#if DEBUG
      this.AttachDevTools();
#endif
    }

    private void OnNavigationChanged(object? sender, NavigationManager.ViewData data)
    {
      var view = m_navigator.ResolveView(data.Name);
      view.SetArguments(data.Arguments);
      view.SetNamedArguments(data.NamedArguments);
      m_mainContent!.Content = view;
    }

    private void InitializeComponent()
    {
      AvaloniaXamlLoader.Load(this);
      m_mainContent = this.FindControl<TransitioningContentControl>("MainContent");
      m_mainContent!.PageTransition = new PageSlide(TimeSpan.FromMilliseconds(500));
    }
  }
}