using Avalonia.Controls;
using MarkDoc.ViewModels;
using MarkDoc.Views;

namespace MarkDoc.MVVM.Helpers
{
  public class BaseUserControl<TViewModel>
    : UserControl, IView<TViewModel>
    where TViewModel : IViewModel
  {
    /// <inheritdoc />
    public TViewModel ViewModel { get; } = TypeResolver.ResolveViewModel<TViewModel>();

    /// <summary>
    /// Default constructor
    /// </summary>
    public BaseUserControl()
      => DataContext = ViewModel;
  }
}