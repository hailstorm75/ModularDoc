using System;

namespace MarkDoc.Core
{
  /// <summary>
  /// Interface for dialog view models
  /// </summary>
  public interface IDialogViewModel
    : IViewModel
  {
    /// <summary>
    /// Invoked when the dialog is to be closed
    /// </summary>
    event EventHandler CloseRequested;

    /// <summary>
    /// Dialog title
    /// </summary>
    string Title { get; }

    /// <summary>
    /// Determines whether the positive button can be clicked
    /// </summary>
    bool CanClickPositive { get; }

    /// <summary>
    /// Determines whether the negative button can be clicked
    /// </summary>
    bool CanClickNegative { get; }

    /// <summary>
    /// Determines whether the cancel button can be clicked
    /// </summary>
    bool CanClickCancel { get; }

    /// <summary>
    /// Invoked when the parent dialog window positive button is pressed
    /// </summary>
    void OnPositiveButtonClicked();

    /// <summary>
    /// Invoked when the parent dialog window negative button is pressed
    /// </summary>
    void OnNegativeButtonClicked();

    /// <summary>
    /// Invoked when the parent dialog window cancel button is pressed
    /// </summary>
    void OnCancelButtonClicked();
  }
}