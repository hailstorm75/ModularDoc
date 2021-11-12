using System.Collections.Generic;

namespace MarkDoc.Core
{
  /// <summary>
  /// Interface for plugin steps
  /// </summary>
  public interface IPluginStep
  {
    /// <summary>
    /// Unique identifier of the step
    /// </summary>
    string Id { get; }

    /// <summary>
    /// Step name
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Step order number
    /// </summary>
    int StepNumber { get; }

    /// <summary>
    /// Determines whether this step is used
    /// </summary>
    bool IsActive { get; set; }

    /// <summary>
    /// Gets the view for this step
    /// </summary>
    /// <param name="settings">Settings to load</param>
    /// <returns>View instance</returns>
    IView GetStepView(IReadOnlyDictionary<string, string> settings);
  }
}