namespace ModularDoc.Core
{
  /// <summary>
  /// Interface for dialog views
  /// </summary>
  public interface IDialogView
    : IView
  {
    /// <summary>
    /// Dialog view title
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

  /// <summary>
  /// Interface for dialog views
  /// </summary>
  /// <typeparam name="TViewModel">View model type for dialog view</typeparam>
  public interface IDialogView<out TViewModel>
    : IView<TViewModel>, IDialogView
    where TViewModel : IDialogViewModel
  {
  }
}