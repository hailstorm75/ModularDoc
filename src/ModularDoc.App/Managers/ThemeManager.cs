using System.Configuration;
using Avalonia;
using FluentAvalonia.Styling;
using ModularDoc.Helpers;

namespace ModularDoc.App.Managers
{
  public class ThemeManager
    : IThemeManager
  {
    private readonly FluentAvaloniaTheme? m_themeManager;

    /// <summary>
    /// Default constructor
    /// </summary>
    public ThemeManager()
    {
      m_themeManager = AvaloniaLocator.Current.GetService<FluentAvaloniaTheme>();
    }

    /// <inheritdoc />
    public bool GetDarkMode() => m_themeManager?.RequestedTheme == "Dark";

    public void SetDarkMode(bool enabled)
    {
      if (m_themeManager is null)
        return;

      m_themeManager.RequestedTheme = enabled
        ? "Dark"
        : "Light";
    }

    public void LoadThemeSettings()
    {
      if (m_themeManager is null)
        return;

      m_themeManager.RequestedTheme =  ConfigurationManager.AppSettings[nameof(m_themeManager.RequestedTheme)] ?? "Light";
    }

    public void SaveThemeSettings()
    {
      if (m_themeManager is null)
        return;

      const string key = nameof(m_themeManager.RequestedTheme);
      var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
      var settings = configFile.AppSettings.Settings;
      if (settings[key] == null)
        settings.Add(key, m_themeManager.RequestedTheme);
      else
        settings[key].Value = m_themeManager.RequestedTheme;

      configFile.Save(ConfigurationSaveMode.Modified);
      ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
    }
  }
}