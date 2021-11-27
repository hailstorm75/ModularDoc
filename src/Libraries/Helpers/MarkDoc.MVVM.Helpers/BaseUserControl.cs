using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Controls;
using MarkDoc.Core;

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

    public async Task SetNamedArgumentsAsync(IReadOnlyDictionary<string, string> arguments)
      => await ViewModel.SetNamedArguments(arguments);
  }
}