namespace ModularDoc.ViewModels.GitMarkdown
{
  public readonly struct Selection
  {
    /// <summary>
    /// Displayed name
    /// </summary>
    public string DisplayName { get; }

    /// <summary>
    /// Option value
    /// </summary>
    public int Value { get; }

    /// <summary>
    /// Default constructor
    /// </summary>
    public Selection(string displayName, int value)
    {
      DisplayName = displayName;
      Value = value;
    }
  }
}