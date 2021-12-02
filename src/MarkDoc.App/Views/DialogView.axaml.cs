using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MarkDoc.Core;
using MarkDoc.Helpers;

namespace MarkDoc.App.Views
{
  public class DialogView
    : Window
  {
    private bool m_result;
    private readonly ContentControl m_mainContent;

    /// <summary>
    /// Displayed dialog buttons
    /// </summary>
    public IDialogManager.DialogButtons DialogButtons { get; set; }

    public IDialogView? ViewContent
    {
      get => (IDialogView)m_mainContent.Content;
      set
      {
        m_mainContent.Content = value;
        Title = value?.Title ?? string.Empty;
      }
    }

    public DialogView()
    {
      AvaloniaXamlLoader.Load(this);
      m_mainContent = this.FindControl<ContentControl>("MainContent");
    }

    public bool GetDialogResult()
    {
      return m_result;
    }

    public void PositiveButton_OnClick(object? sender, RoutedEventArgs e)
    {
      ViewContent!.OnPositiveButtonClicked();
      m_result = true;
      Close();
    }

    public void NegativeButton_OnClick(object? sender, RoutedEventArgs e)
    {
      ViewContent!.OnNegativeButtonClicked();
      m_result = false;
      Close();
    }

    public void CancelButton_OnClick(object? sender, RoutedEventArgs e)
    {
      ViewContent!.OnCancelButtonClicked();
      m_result = false;
      Close();
    }
  }
}