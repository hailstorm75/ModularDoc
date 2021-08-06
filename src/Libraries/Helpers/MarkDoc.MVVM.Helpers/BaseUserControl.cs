using System.Collections.Generic;
using Avalonia.Controls;
using MarkDoc.ViewModels;
using MarkDoc.Views;

namespace MarkDoc.MVVM.Helpers
{
  public abstract class BaseUserControl<TViewModel>
    : UserControl, IView<TViewModel>
    where TViewModel : IViewModel
  {
    /// <inheritdoc />
    public TViewModel ViewModel { get; } = TypeResolver.ResolveViewModel<TViewModel>();

    /// <summary>
    /// Default constructor
    /// </summary>
    protected BaseUserControl()
      => DataContext = ViewModel;

    public void SetNamedArguments(IReadOnlyDictionary<string, string> arguments)
      => ViewModel.SetNamedArguments(arguments);
  }
}