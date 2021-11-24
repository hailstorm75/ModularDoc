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
    /// Determines whether this step is last in the step sequence
    /// </summary>
    bool IsLastStep { get; }

    /// <summary>
    /// Gets the view for this step
    /// </summary>
    /// <param name="settings">Settings to load</param>
    /// <returns>View instance</returns>
    IStepView<IStepViewModel> GetStepView(IReadOnlyDictionary<string, string> settings);
  }
}