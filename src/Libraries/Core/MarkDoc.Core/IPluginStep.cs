namespace MarkDoc.Core
{
  /// <summary>
  /// Interface for plugin steps
  /// </summary>
  public interface IPluginStep
  {
    /// <summary>
    /// Step name
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Step order number
    /// </summary>
    int StepNumber { get; }

    /// <summary>
    /// Gets the view for this step
    /// </summary>
    /// <param name="settings">Settings to use</param>
    /// <returns>View instance</returns>
    IView GetStepView(ILibrarySettings? settings = default);
  }
}