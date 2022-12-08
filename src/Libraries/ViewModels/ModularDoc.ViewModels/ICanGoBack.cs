using System.Windows.Input;

namespace ModularDoc.ViewModels
{
  /// <summary>
  /// Interface for view models with the back navigation feature
  /// </summary>
  public interface ICanGoBack
  {
    /// <summary>
    /// Command for navigating to the previous view
    /// </summary>
    ICommand BackCommand { get; }
  }
}