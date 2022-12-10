using System;
using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ModularDoc;
using ModularDoc.Core;
using ModularDoc.Helpers;
using ReactiveUI;

namespace ModularDoc.App.Views
{
  public class DialogViewModel
    : ReactiveObject
  {
    #region Fields

    private readonly Action m_closeMethod;
    private bool m_cancelVisible;
    private bool m_positiveVisible;
    private bool m_negativeVisible;
    private string m_title = string.Empty;
    private IDialogView? m_viewContent;

    #endregion

    #region Properties

    /// <summary>
    /// Dialog title
    /// </summary>
    public string Title
    {
      get => m_title;
      set
      {
        m_title = value;
        this.RaisePropertyChanged(nameof(Title));
      }
    }

    public IDialogView? ViewContent
    {
      get => m_viewContent;
      set
      {
        m_viewContent = value;
        Title = value?.Title ?? string.Empty;

        if (m_viewContent is null)
          return;

        var viewModel = m_viewContent.GetViewModel() as IDialogViewModel;
        var canExecuteCancelCommand = viewModel.WhenAnyValue(vm => vm.CanClickCancel);
        var canExecutePositiveCommand = viewModel.WhenAnyValue(vm => vm.CanClickPositive);
        var canExecuteNegativeCommand = viewModel.WhenAnyValue(vm => vm.CanClickNegative);

        CancelClickCommand = ReactiveCommand.Create(CancelClick, canExecuteCancelCommand);
        PositiveClickCommand = ReactiveCommand.Create(PositiveClick, canExecutePositiveCommand);
        NegativeClickCommand = ReactiveCommand.Create(NegativeClick, canExecuteNegativeCommand);
      }
    }

    /// <summary>
    /// Displayed dialog buttons
    /// </summary>
    public IDialogManager.DialogButtons DialogButtons { get; set; }

    /// <summary>
    /// Determines whether the cancel button is visible
    /// </summary>
    public bool CancelVisible
    {
      get => m_cancelVisible;
      set
      {
        m_cancelVisible = value;
        this.RaisePropertyChanged(nameof(CancelVisible));
      }
    }

    /// <summary>
    /// Determines whether the positive button is visible
    /// </summary>
    public bool PositiveVisible
    {
      get => m_positiveVisible;
      set
      {
        m_positiveVisible = value;
        this.RaisePropertyChanged(nameof(PositiveVisible));
      }
    }

    /// <summary>
    /// Determines whether the negative button is visible
    /// </summary>
    public bool NegativeVisible
    {
      get => m_negativeVisible;
      set
      {
        m_negativeVisible = value;
        this.RaisePropertyChanged(nameof(NegativeVisible));
      }
    }

    #endregion

    #region Commands

    public ICommand? CancelClickCommand { get; private set; }

    public ICommand? PositiveClickCommand { get; private set; }

    public ICommand? NegativeClickCommand { get; private set; }

    #endregion

    /// <summary>
    /// Default constructor
    /// </summary>
    public DialogViewModel(Action closeMethod)
    {
      m_closeMethod = closeMethod;
    }

    #region Methods

    private void CancelClick()
    {
      ViewContent?.OnCancelButtonClicked();
      m_closeMethod();
    }

    private void PositiveClick()
    {
      ViewContent?.OnPositiveButtonClicked();
      m_closeMethod();
    }

    private void NegativeClick()
    {
      ViewContent?.OnNegativeButtonClicked();
      m_closeMethod();
    }

    #endregion
  }

  public class DialogView
    : Window
  {
    private readonly ContentControl m_mainContent;

    public DialogView()
    {
      AvaloniaXamlLoader.Load(this);
      m_mainContent = this.FindControl<ContentControl>("MainContent");
      DataContext = new DialogViewModel(Close);
    }

    public void SetViewContent(IDialogView view)
    {
      ((DialogViewModel)DataContext!).ViewContent = view;
      m_mainContent.Content = view;
    }

    public bool GetDialogResult()
    {
      return true;
    }
  }
}
