using System.Windows.Input;
using MarkDoc.Core;

namespace MarkDoc.ViewModels
{
  /// <summary>
  /// Interface for settings view models
  /// </summary>
  public interface ISettingsViewModel
    : IViewModel
  {
    /// <summary>
    /// Navigates back to the home page
    /// </summary>
    ICommand BackCommand { get; set; }
  }
}