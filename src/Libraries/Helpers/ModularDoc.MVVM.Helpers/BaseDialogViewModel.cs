using System;
using ModularDoc;
using ReactiveUI;

namespace ModularDoc.MVVM.Helpers
{
  public abstract class BaseDialogViewModel
    : BaseViewModel, IDialogViewModel
  {
    /// <inheritdoc />
    public event EventHandler? CloseRequested;

    #region Fields

    private string m_title = string.Empty;
    private bool m_canClickPositive;
    private bool m_canClickNegative;
    private bool m_canClickCancel;

    #endregion

    #region Properties

    /// <inheritdoc />
    public string Title
    {
      get => m_title;
      set
      {
        m_title = value;
        this.RaisePropertyChanged(nameof(Title));
      }
    }

    /// <inheritdoc />
    public bool CanClickPositive
    {
      get => m_canClickPositive;
      set
      {
        m_canClickPositive = value;
        this.RaisePropertyChanged(nameof(CanClickPositive));
      }
    }

    /// <inheritdoc />
    public bool CanClickNegative
    {
      get => m_canClickNegative;
      set
      {
        m_canClickNegative = value;
        this.RaisePropertyChanged(nameof(CanClickNegative));
      }
    }

    /// <inheritdoc />
    public bool CanClickCancel
    {
      get => m_canClickCancel;
      set
      {
        m_canClickCancel = value;
        this.RaisePropertyChanged(nameof(m_canClickCancel));
      }
    }

    /// <inheritdoc />
    public abstract void OnPositiveButtonClicked();

    /// <inheritdoc />
    public abstract void OnNegativeButtonClicked();

    /// <inheritdoc />
    public abstract void OnCancelButtonClicked();

    #endregion

    /// <summary>
    /// Requests the dialog to be closed
    /// </summary>
    protected void RaiseCloseRequest()
      => CloseRequested?.Invoke(this, EventArgs.Empty);
  }
}