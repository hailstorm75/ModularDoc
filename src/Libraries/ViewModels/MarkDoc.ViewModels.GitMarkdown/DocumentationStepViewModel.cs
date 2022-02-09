using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using MarkDoc.Documentation;
using MarkDoc.Members;
using MarkDoc.MVVM.Helpers;

namespace MarkDoc.ViewModels.GitMarkdown
{
  public sealed class DocumentationStepViewModel
    : BaseStepViewModel<IDocSettings>
  {
    #region Constants

    private const string SETTINGS_PATHS = "paths";
    private const char PATH_DELIM = '|';

    #endregion

    #region Properties

    /// <inheritdoc />
    public override string Id => "C5C5D04A-7975-468A-ACB7-251F20DC4D4C";

    /// <inheritdoc />
    public override string Title => "Documentation extraction settings";

    /// <inheritdoc />
    public override string Description => "Validate that there are no missing documentation files. If documentation is missing, provide the path manually, or explicitly choose to ignore missing documentation.";

    /// <summary>
    /// Paths to documentation
    /// </summary>
    public ObservableCollection<string> Paths { get; } = new();

    /// <summary>
    /// Missing paths to documentation
    /// </summary>
    public ObservableCollection<string> MissingPaths { get; } = new();

    #endregion

    /// <summary>
    /// Default constructor
    /// </summary>
    public DocumentationStepViewModel()
      => MissingPaths.CollectionChanged += (_, _) => UpdateCanProceed();

    /// <inheritdoc />
    public override Task SetNamedArguments(IReadOnlyDictionary<string, string> arguments)
    {
      // throw new System.NotImplementedException();
      return Task.CompletedTask;
    }

    /// <inheritdoc />
    public override async ValueTask SetPreviousSettings(
      IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> settings)
    {
      await base.SetPreviousSettings(settings).ConfigureAwait(false);

      if (!settings.TryGetValue(AssemblyStepViewModel.ID, out var pluginSettings))
        return;

      // ReSharper disable once AssignNullToNotNullAttribute
      LoadDocumentationPaths(pluginSettings);
    }

    private void LoadDocumentationPaths(IReadOnlyDictionary<string, string> settings)
    {
      if (!settings.TryGetValue(IMemberSettings.ENTRY_PATHS, out var data))
        return;

      // ReSharper disable once PossibleNullReferenceException
      var paths = data.Split(IMemberSettings.PATH_DELIMITER);
      foreach (var path in paths)
      {
        var documentation = path.Remove(path.Length - 3) + "xml";
        if (File.Exists(documentation))
          Paths.Add(documentation);
        // Otherwise..
        else
          MissingPaths.Add(documentation);
      }

      UpdateCanProceed();
    }

    /// <inheritdoc />
    public override IReadOnlyDictionary<string, string> GetSettings()
      => new Dictionary<string, string>
      {
        {
          SETTINGS_PATHS, string.Join(PATH_DELIM, Paths)
        }
      };

    private void UpdateCanProceed()
      => IsValid = MissingPaths.Count == 0;
  }
}