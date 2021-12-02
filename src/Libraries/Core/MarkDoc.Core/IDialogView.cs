namespace MarkDoc.Core
{
  /// <summary>
  /// Interface for dialog views
  /// </summary>
  /// <typeparam name="TViewModel">View model type for dialog view</typeparam>
  public interface IDialogView<out TViewModel>
    : IDialogView
    where TViewModel : IDialogViewModel
  {
  }

  /// <summary>
  /// Interface for dialog views
  /// </summary>
  public interface IDialogView
    : IView<IDialogViewModel>
  {
    /// <summary>
    /// Dialog view title
    /// </summary>
    public string Title => ViewModel.Title;

    /// <summary>
    /// Invoked when the parent dialog window positive button is pressed
    /// </summary>
    public void OnPositiveButtonClicked()
      => ViewModel.OnPositiveButtonClicked();

    /// <summary>
    /// Invoked when the parent dialog window negative button is pressed
    /// </summary>
    public void OnNegativeButtonClicked()
      => ViewModel.OnNegativeButtonClicked();

    /// <summary>
    /// Invoked when the parent dialog window cancel button is pressed
    /// </summary>
    public void OnCancelButtonClicked()
      => ViewModel.OnCancelButtonClicked();
  }
}