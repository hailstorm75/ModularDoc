using MarkDoc.ViewModels;

namespace MarkDoc.Views
{
  public interface IView<out TViewModel>
    where TViewModel : IViewModel
  {
    /// <summary>
    /// View model
    /// </summary>
    public TViewModel ViewModel { get; }
  }
}
