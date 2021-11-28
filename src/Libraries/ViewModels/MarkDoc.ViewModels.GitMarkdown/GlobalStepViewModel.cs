using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using MarkDoc.Core;
using MarkDoc.Members;
using MarkDoc.MVVM.Helpers;

namespace MarkDoc.ViewModels.GitMarkdown
{
  public class GlobalStepViewModel
    : BaseStepViewModel<IGlobalSettings>
  {
    private int m_totalTypes;
    private readonly HashSet<TypeNode> m_disabledTypeNodes = new();
    private readonly IResolver m_resolver;
    private readonly ISettingsCreator m_settingsCreator;
    private readonly List<BaseTrieNode> m_allNodes = new();
    private readonly TaskCompletionSource<IReadOnlyDictionary<string, string>> m_arguments = new();

    /// <inheritdoc />
    public override string Id => "23407B59-027B-43F9-901C-57F3016DE237";

    /// <inheritdoc />
    public override string Title => "Global settings";

    /// <inheritdoc />
    public override string Description => "Settings used by multiple components";

    /// <summary>
    /// Namespaces inside the libraries
    /// </summary>
    public ObservableCollection<NamespaceNode> AvailableNamespaces { get; } = new();

    /// <summary>
    /// Default constructor
    /// </summary>
    public GlobalStepViewModel(IResolver resolver, ISettingsCreator settingsCreator)
    {
      IsValid = true;
      m_resolver = resolver;
      m_settingsCreator = settingsCreator;
    }

    ~GlobalStepViewModel()
    {
      foreach (var space in AvailableNamespaces)
        space.PropertyChanged -= RootOnPropertyChanged;
    }

    /// <inheritdoc />
    public override IReadOnlyDictionary<string, string> GetSettings()
      => new Dictionary<string, string>
      {
        {
          nameof(IGlobalSettings.IgnoredNamespaces),
          string.Join('|', m_allNodes.OfType<NamespaceNode>().Where(node => !node.IsEnabled || !node.IsChecked).Select(node => node.FullName))
        },
        {
          nameof(IGlobalSettings.IgnoredTypes),
          string.Join('|', m_allNodes.OfType<TypeNode>().Where(node => !node.IsEnabled || !node.IsChecked).Select(node => node.FullName))
        }
      };

    /// <inheritdoc />
    public override Task SetNamedArguments(IReadOnlyDictionary<string, string> arguments)
    {
      m_arguments.TrySetResult(arguments);
      return Task.CompletedTask;
    }

    /// <inheritdoc />
    public override async ValueTask SetPreviousSettings(IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> settings)
    {
      await base.SetPreviousSettings(settings).ConfigureAwait(false);

      if (!settings.TryGetValue(AssemblyStepViewModel.ID, out var data))
        return;

      // ReSharper disable once RedundantSuppressNullableWarningExpression
      var memberSettings = m_settingsCreator.CreateSettings<IMemberSettings>(data!);
      var globalSettings = m_settingsCreator.CreateSettings<IGlobalSettings>(new Dictionary<string, string>());

      await m_resolver.ResolveAsync(memberSettings, globalSettings);

      var (roots, allNodes) = NamespaceNode.CreateNodes(m_resolver);
      m_allNodes.AddRange(allNodes);

      foreach (var root in roots)
      {
        AvailableNamespaces.Add(root);

        root.PropertyChanged += RootOnPropertyChanged;
      }

      foreach (var node in allNodes.OfType<TypeNode>())
      {
        ++m_totalTypes;

        node.PropertyChanged += TypeNodeOnPropertyChanging;
      }

      var arguments = await m_arguments.Task.ConfigureAwait(false);
      if (arguments.TryGetValue(nameof(IGlobalSettings.IgnoredNamespaces), out var ignoredNamespaces))
      {
        // ReSharper disable once PossibleNullReferenceException
        var spaces = ignoredNamespaces.Split('|').ToHashSet();
        var allNamespaces = m_allNodes.OfType<NamespaceNode>().Where(node => spaces.Contains(node.FullName));
        foreach (var node in allNamespaces)
          node.IsChecked = false;
      }

      if (arguments.TryGetValue(nameof(IGlobalSettings.IgnoredTypes), out var ignoredTypes))
      {
        // ReSharper disable once PossibleNullReferenceException
        var types = ignoredTypes.Split('|').ToHashSet();
        var allTypes = m_allNodes.OfType<TypeNode>().Where(node => types.Contains(node.FullName));
        foreach (var node in allTypes)
          node.IsChecked = false;
      }
    }

    private void TypeNodeOnPropertyChanging(object? sender, PropertyChangedEventArgs e)
    {
      var node = sender as TypeNode;

      switch (e.PropertyName)
      {
        case nameof(TypeNode.IsEnabled):
        case nameof(TypeNode.IsChecked):
          if (!node!.IsEnabled || !node.IsChecked)
            m_disabledTypeNodes.Add(node);
          else
            m_disabledTypeNodes.Remove(node);

          UpdateIsValid();
          break;
      }
    }

    private void RootOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
      switch (e.PropertyName)
      {
        case nameof(BaseTrieNode.IsChecked):
          UpdateIsValid();
          break;
      }
    }

    private void UpdateIsValid()
      => IsValid = AvailableNamespaces.Any(space => space.IsChecked) && m_totalTypes != m_disabledTypeNodes.Count;
  }
}