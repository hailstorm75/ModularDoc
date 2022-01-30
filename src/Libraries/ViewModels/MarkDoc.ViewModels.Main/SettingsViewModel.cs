using System.Windows.Input;
using MarkDoc.Constants;
using MarkDoc.MVVM.Helpers;
using ReactiveUI;

namespace MarkDoc.ViewModels.Main
{
  public class SettingsViewModel
    : BaseViewModel, ISettingsViewModel
  {
    private readonly NavigationManager m_navigationManager;

    /// <inheritdoc />
    public ICommand BackCommand { get; set; }

    /// <summary>
    /// Default constructor
    /// </summary>
    public SettingsViewModel(NavigationManager navigationManager)
    {
      m_navigationManager = navigationManager;
      BackCommand = ReactiveCommand.Create(NavigateBack);
    }

    private void NavigateBack()
      => m_navigationManager.NavigateTo(PageNames.HOME);
  }
}