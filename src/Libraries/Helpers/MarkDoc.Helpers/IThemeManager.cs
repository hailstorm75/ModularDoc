namespace MarkDoc.Helpers
{
  public interface IThemeManager
  {
    bool GetDarkMode();
    void SetDarkMode(bool enabled);
    void SaveThemeSettings();
    void LoadThemeSettings();
  }
}