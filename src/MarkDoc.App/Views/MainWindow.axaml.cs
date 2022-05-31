using System;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using FluentAvalonia.Styling;
using FluentAvalonia.UI.Controls;
using MarkDoc.Constants;
using MarkDoc.Helpers;
using MarkDoc.MVVM.Helpers;

namespace MarkDoc.App.Views
{
  public class MainWindow
    : CoreWindow
  {
    private readonly NavigationManager m_navigator;
    private TransitioningContentControl? m_mainContent;
    //private readonly IPageTransition m_forward;
    private readonly IPageTransition m_back;

    public MainWindow()
    {
      m_navigator = TypeResolver.Resolve<NavigationManager>();
      m_navigator.NavigationChanged += OnNavigationChanged;
      InitializeComponent();

      //m_forward = new PageSlide(TimeSpan.FromMilliseconds(400));
      m_back = new CrossFade(TimeSpan.FromMilliseconds(120));
      ((CrossFade)m_back).FadeInEasing = new QuadraticEaseIn();
      m_navigator.NavigateTo(PageNames.HOME);

#if DEBUG
      this.AttachDevTools();
#endif
    }

    /// <inheritdoc />
    protected override void OnOpened(EventArgs e)
    {
      base.OnOpened(e);

      // Check if CoreWindow has ICoreApplicationViewTitleBar (null only on Mac/Linux)
      if (TitleBar != null)
      {
        // Tell CoreWindow we want to remove the default TitleBar and set our own
        TitleBar.ExtendViewIntoTitleBar = true;

        // Retrieve reference to the Xaml element we're using a TitleBar
        if (this.FindControl<Grid>("TitleBarHost") is { } g)
        {
          // Call SetTitleBar to tell CoreWindow the element we want to use as the TitleBar
          SetTitleBar(g);
          g.IsVisible = true;

          // Set the margin of the Custom TitleBar so it doesn't overlap with the CaptionButtons
          g.Margin = new Thickness(8, 5, TitleBar.SystemOverlayRightInset, 5);
        }
      }

      var faTheme = AvaloniaLocator.Current.GetService<FluentAvaloniaTheme>();
      if (faTheme == null)
        return;

      var themeManager = TypeResolver.Resolve<IThemeManager>();
      themeManager.LoadThemeSettings();

      faTheme.RequestedTheme = themeManager.GetDarkMode() ? "Dark" : "Light";
      faTheme.UseUserAccentColorOnWindows = true;
      faTheme.ForceWin32WindowToTheme(this);
    }

    private async void OnNavigationChanged(object? sender, NavigationManager.ViewData data)
    {
      var view = m_navigator.ResolveView(data.Name);

      if (data.Arguments.Count != 0)
        await view.SetArguments(data.Arguments).ConfigureAwait(false);

      if (data.NamedArguments.Count != 0)
        await view.SetNamedArgumentsAsync(data.NamedArguments).ConfigureAwait(true);

      m_mainContent!.Content = view;

      m_mainContent!.PageTransition = m_back;
    }

    private void InitializeComponent()
    {
      AvaloniaXamlLoader.Load(this);
      m_mainContent = this.FindControl<TransitioningContentControl>("MainContent");
      m_mainContent!.PageTransition = m_back;
    }
  }
}