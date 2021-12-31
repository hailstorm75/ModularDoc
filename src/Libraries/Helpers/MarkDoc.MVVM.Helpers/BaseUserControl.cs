using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Controls;
using MarkDoc.Core;

namespace MarkDoc.MVVM.Helpers
{
  public abstract class BaseDialogUserControl<TViewModel>
    : BaseUserControl<TViewModel>, IDialogView
    where TViewModel : IDialogViewModel
  {
    /// <summary>
    /// Dialog view title
    /// </summary>
    public string Title => ViewModel.Title;

    /// <summary>
    /// Invoked when the parent dialog window positive button is pressed
    /// </summary>
    public void OnPositiveButtonClicked()
      => ViewModel.OnPositiveButtonClicked();

    /// <summary>
    /// Invoked when the parent dialog window negative button is pressed
    /// </summary>
    public void OnNegativeButtonClicked()
      => ViewModel.OnNegativeButtonClicked();

    /// <summary>
    /// Invoked when the parent dialog window cancel button is pressed
    /// </summary>
    public void OnCancelButtonClicked()
      => ViewModel.OnCancelButtonClicked();
  }

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

    // ReSharper disable once AnnotateNotNullTypeMember
    public IViewModel GetViewModel()
      => ViewModel;

    /// <inheritdoc />
    protected override async void OnInitialized()
    {
      base.OnInitialized();

      await ViewModel.OnLoadedAsync().ConfigureAwait(false);
    }

    public async Task SetNamedArgumentsAsync(IReadOnlyDictionary<string, string> arguments)
      => await ViewModel.SetNamedArguments(arguments);
  }
}