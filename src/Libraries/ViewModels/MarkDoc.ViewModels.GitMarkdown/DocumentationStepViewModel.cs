using System.Collections.Generic;
using System.Collections.ObjectModel;
using MarkDoc.Documentation;
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
    public override string Description => "TODO";

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
    public override void SetNamedArguments(IReadOnlyDictionary<string, string> arguments)
    {
      // throw new System.NotImplementedException();
    }

    /// <inheritdoc />
    public override void SetPreviousSettings(IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> settings)
    {
      base.SetPreviousSettings(settings);
      
      
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
      => IsValid = MissingPaths.Count > 0;
  }
}