namespace MarkDoc.Core
{
  /// <summary>
  /// Interface for dialog view models
  /// </summary>
  public interface IDialogViewModel
    : IViewModel
  {
    /// <summary>
    /// Dialog title
    /// </summary>
    string Title { get; }

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