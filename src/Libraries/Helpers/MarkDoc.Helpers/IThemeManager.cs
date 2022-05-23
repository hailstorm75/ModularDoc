namespace MarkDoc.Helpers
{
  public interface IThemeManager
  {
    /// <summary>
    /// Retrieves the current state of the Dark mode
    /// </summary>
    /// <returns></returns>
    bool GetDarkMode();

    /// <summary>
    /// Sets the Dark mode state
    /// </summary>
    /// <param name="enabled">True if Dark mode is to be enabled</param>
    void SetDarkMode(bool enabled);

    /// <summary>
    /// Saves all current theme settings
    /// </summary>
    void SaveThemeSettings();

    /// <summary>
    /// Loads all current theme settings
    /// </summary>
    void LoadThemeSettings();
  }
}