using System.Windows.Input;
using ModularDoc.Constants;
using ModularDoc.Helpers;
using ModularDoc.MVVM.Helpers;
using ReactiveUI;

namespace ModularDoc.ViewModels.Main
{
  public class SettingsViewModel
    : BaseViewModel, ISettingsViewModel
  {
    #region Field

    private readonly NavigationManager m_navigationManager;
    private readonly IThemeManager m_themeManager;
    private bool m_isDarkModeOn;

    #endregion

    /// <summary>
    /// Determines whether the dark mode is on
    /// </summary>
    public bool IsDarkModeOn
    {
      get => m_isDarkModeOn;
      set
      {
        m_isDarkModeOn = value;
        SetDarkMode(value);
        this.RaisePropertyChanged();
      }
    }

    /// <inheritdoc />
    public ICommand BackCommand { get; }

    /// <summary>
    /// Default constructor
    /// </summary>
    public SettingsViewModel(NavigationManager navigationManager, IThemeManager themeManager)
    {
      m_navigationManager = navigationManager;
      m_themeManager = themeManager;
      BackCommand = ReactiveCommand.Create(NavigateBack);
      IsDarkModeOn = m_themeManager.GetDarkMode();
    }

    private void SetDarkMode(bool enabled)
      => m_themeManager.SetDarkMode(enabled);

    private void NavigateBack()
    {
      m_themeManager.SaveThemeSettings();
      m_navigationManager.NavigateTo(PageNames.HOME);
    }
  }
}