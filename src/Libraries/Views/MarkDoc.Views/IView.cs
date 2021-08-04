using MarkDoc.ViewModels;
using System.Collections.Generic;

namespace MarkDoc.Views
{
  public interface IView<out TViewModel>
    : IView
    where TViewModel : IViewModel
  {
    /// <summary>
    /// View model
    /// </summary>
    public TViewModel ViewModel { get; }
  }

  public interface IView
  {
    void SetArguments(IReadOnlyCollection<string> arguments);
    void SetNamedArguments(IReadOnlyDictionary<string, string> arguments);
  }
}
