using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MarkDoc.Core;
using MarkDoc.Members;
using MarkDoc.MVVM.Helpers;
using Moq;

namespace MarkDoc.ViewModels.GitMarkdown
{
  public class GlobalStepViewModel
    : BaseStepViewModel<IGlobalSettings>
  {
    private readonly IResolver m_resolver;
    private readonly ISettingsCreator m_settingsCreator;

    /// <inheritdoc />
    public override string Id => "23407B59-027B-43F9-901C-57F3016DE237";

    /// <inheritdoc />
    public override string Title => "Global settings";

    /// <inheritdoc />
    public override string Description => "Settings used by multiple components";

    /// <summary>
    /// Namespaces inside the libraries
    /// </summary>
    public ObservableCollection<string> AvailableNamespaces { get; } = new();

    /// <summary>
    /// Collection of ignored namespaces
    /// </summary>
    public ObservableCollection<string> IgnoredNamespaces { get; } = new();

    /// <summary>
    /// Collection of ignored types
    /// </summary>
    public ObservableCollection<string> IgnoredTypes { get; } = new();

    /// <summary>
    /// Default constructor
    /// </summary>
    public GlobalStepViewModel(IResolver resolver, ISettingsCreator settingsCreator)
    {
      m_resolver = resolver;
      m_settingsCreator = settingsCreator;
    }

    /// <inheritdoc />
    public override IReadOnlyDictionary<string, string> GetSettings()
      => new Dictionary<string, string>
      {
        {
          nameof(IGlobalSettings.IgnoredNamespaces),
          string.Join('|', IgnoredNamespaces)
        },
        {
          nameof(IGlobalSettings.IgnoredTypes),
          string.Join('|', IgnoredTypes)
        }
      };

    /// <inheritdoc />
    public override Task SetNamedArguments(IReadOnlyDictionary<string, string> arguments) => Task.CompletedTask;

    /// <inheritdoc />
    public override async ValueTask SetPreviousSettings(IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> settings)
    {
      await base.SetPreviousSettings(settings).ConfigureAwait(false);

      if (!settings.TryGetValue(AssemblyStepViewModel.ID, out var data))
        return;

      var memberSettings = m_settingsCreator.CreateSettings<IMemberSettings>(data!);
      var globalSettings = new Mock<IGlobalSettings>();
      globalSettings.SetupProperty(x => x.IgnoredNamespaces, Array.Empty<string>());
      globalSettings.SetupProperty(x => x.IgnoredTypes, Array.Empty<string>());

      await m_resolver.ResolveAsync(memberSettings, globalSettings.Object);

      foreach (var entry in m_resolver.Types.Value)
        AvailableNamespaces.Add(entry.Key);
    }
  }
}