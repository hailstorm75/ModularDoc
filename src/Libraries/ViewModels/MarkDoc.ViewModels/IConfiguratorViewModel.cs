using MarkDoc.Core;

namespace MarkDoc.ViewModels
{
  /// <summary>
  /// Interface for configurator view models
  /// </summary>
  public interface IConfiguratorViewModel
    : IViewModel, ICanGoBack
  {
    /// <summary>
    /// Plugin title
    /// </summary>
    string Title { get; }
  }
}