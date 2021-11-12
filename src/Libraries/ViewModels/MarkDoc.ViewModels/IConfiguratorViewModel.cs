using System.Collections.Generic;
using System.Collections.ObjectModel;
using MarkDoc.Core;

namespace MarkDoc.ViewModels
{
  /// <summary>
  /// Interface for configurator view models
  /// </summary>
  public interface IConfiguratorViewModel
    : IViewModel, ICanGoBack
  {
    public const string ARGUMENT_ID = "id";
    public const string ARGUMENT_SETTINGS = "settings";

    /// <summary>
    /// Plugin title
    /// </summary>
    string Title { get; }

    ObservableCollection<IPluginStep> Steps { get; }

    /// <summary>
    /// Currently active step
    /// </summary>
    IPluginStep? CurrentStep { get; }

    Dictionary<string, string> CurrentStepSettings { get; }

    IReadOnlyDictionary<string, Dictionary<string, string>> PluginSettings { get; }
  }
}