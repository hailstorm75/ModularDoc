using System.Threading.Tasks;
using System.Windows.Input;
using MarkDoc.Core;

namespace MarkDoc.ViewModels
{
  /// <summary>
  /// Interface for summary view models
  /// </summary>
  public interface ISummaryViewModel
    : IViewModel
  {
    /// <summary>
    /// View title
    /// </summary>
    string Title { get; }

    /// <summary>
    /// Command for navigating back
    /// </summary>
    ICommand BackCommand { get; }

    /// <inheritdoc />
    Task ExecuteAsync();
  }
}