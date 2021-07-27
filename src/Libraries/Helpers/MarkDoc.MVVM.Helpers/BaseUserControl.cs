using System.Collections.Generic;
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

    public void SetArguments(IReadOnlyCollection<string> arguments)
      => ViewModel.SetArguments(arguments);

    public void SetNamedArguments(IReadOnlyDictionary<string, string> arguments)
      => ViewModel.SetNamedArguments(arguments);
  }
}