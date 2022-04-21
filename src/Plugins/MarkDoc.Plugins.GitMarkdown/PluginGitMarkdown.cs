using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using MarkDoc.Core;
using MarkDoc.Diagrams;
using MarkDoc.Diagrams.Mermaid;
using MarkDoc.Documentation;
using MarkDoc.Documentation.Xml;
using MarkDoc.Elements;
using MarkDoc.Elements.Markdown;
using MarkDoc.Generator;
using MarkDoc.Generator.Basic;
using MarkDoc.Helpers;
using MarkDoc.Linkers;
using MarkDoc.Linkers.Markdown;
using MarkDoc.Members;
using MarkDoc.Members.Dnlib;
using MarkDoc.Members.Types;
using MarkDoc.MVVM.Helpers;
using MarkDoc.Printer;
using MarkDoc.Printer.Markdown;
using MarkDoc.ViewModels.GitMarkdown;
using MarkDoc.Views.GitMarkdown;
using Module = Autofac.Module;

namespace MarkDoc.Plugins.GitMarkdown
{
  public sealed class PluginGitMarkdown
    : Module, IPlugin
  {
    private readonly ISettingsCreator m_settingsCreator;
    private static readonly Lazy<IReadOnlyCollection<string>> STEPS;

    #region Properties

    /// <inheritdoc />
    public string Id => "34C7AA14-E6FE-4684-BBE3-03C00F567297";

    /// <inheritdoc />
    public string Name => "Markdown for Git";

    /// <inheritdoc />
    public string Description => "Markdown documentation generating plugin for GitHub, GitLab, and Bitbucket";

    /// <inheritdoc />
    public string Author => "MarkDoc";

    /// <inheritdoc />
    public Stream? Image => Assembly.GetExecutingAssembly().GetManifestResourceStream("MarkDoc.Plugins.GitMarkdown.icon.png");

    /// <inheritdoc />
    public IReadOnlyCollection<string> Steps => STEPS.Value;

    #endregion

    static PluginGitMarkdown()
    {
      STEPS = new Lazy<IReadOnlyCollection<string>>(() => TypeResolver
        .Resolve<IEnumerable<IPluginStep>>()
        .OrderBy(step => step.StepNumber)
        .Select(step => step.Name)
        .ToReadOnlyCollection());
    }

    /// <summary>
    /// Default constructor
    /// </summary>
    public PluginGitMarkdown()
      => m_settingsCreator = new SettingsCreator();

    /// <inheritdoc />
    protected override void Load(ContainerBuilder builder)
    {
      builder.RegisterType<PluginGitMarkdown>().As<IPlugin>();
      builder.RegisterType<Resolver>().As<IResolver>().SingleInstance();
      builder.RegisterType<DocResolver>().As<IDocResolver>().SingleInstance();
      builder.RegisterType<Creator>().As<IElementCreator>().SingleInstance();
      builder.RegisterType<Linker>().As<ILinker>().SingleInstance();
      builder.RegisterType<TypeComposer>().As<ITypeComposer>().SingleInstance();
      builder.RegisterType<PrinterMarkdown>().As<IPrinter>().SingleInstance();
      builder.RegisterType<MermaidResolver>().As<IDiagramResolver>().SingleInstance();

      builder.RegisterType<AssembliesStep>().As<IPluginStep>();
      builder.RegisterType<DocumentationStep>().As<IPluginStep>();
      builder.RegisterType<LinkerStep>().As<IPluginStep>();
      builder.RegisterType<GlobalStep>().As<IPluginStep>();

      builder.RegisterType<AssemblyStepView>().As<IStepView<IStepViewModel<IMemberSettings>, IMemberSettings>>();
      builder.RegisterType<AssemblyStepViewModel>().As<IStepViewModel<IMemberSettings>>();
      builder.RegisterType<DocumentationStepView>().As<IStepView<IStepViewModel<IDocSettings>, IDocSettings>>();
      builder.RegisterType<DocumentationStepViewModel>().As<IStepViewModel<IDocSettings>>();
      builder.RegisterType<LinkerStepView>().As<IStepView<IStepViewModel<ILinkerSettings>, ILinkerSettings>>();
      builder.RegisterType<LinkerStepViewModel>().As<IStepViewModel<ILinkerSettings>>();
      builder.RegisterType<GlobalStepView>().As<IStepView<IStepViewModel<IGlobalSettings>, IGlobalSettings>>();
      builder.RegisterType<GlobalStepViewModel>().As<IStepViewModel<IGlobalSettings>>();

      builder.RegisterType<SettingsCreator>().As<ISettingsCreator>();
      builder.RegisterInstance(new DefiniteProcess(string.Empty, 0)).As<IDefiniteProcess>();
    }

    /// <inheritdoc />
    public IReadOnlyCollection<IPluginStep> GetPluginSteps() => TypeResolver
      .Resolve<IEnumerable<IPluginStep>>()
      .OrderBy(step => step.StepNumber)
      .ToReadOnlyCollection();

    /// <inheritdoc />
    // ReSharper disable once AnnotateNotNullTypeMember
    public T GetSettings<T>(IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> data) where T : ILibrarySettings
    {
      switch (typeof(T).Name)
      {
        case "IMemberSettings":
          data.TryGetValue(AssembliesStep.ID, out var assemblyData);
          return m_settingsCreator.CreateSettings<T>(assemblyData ?? new Dictionary<string, string>());
        case "IDocSettings":
          data.TryGetValue(DocumentationStep.ID, out var documentationData);
          return m_settingsCreator.CreateSettings<T>(documentationData ?? new Dictionary<string, string>());
        case "ILinkerSettings":
          data.TryGetValue(LinkerStep.ID, out var linkerData);
          return m_settingsCreator.CreateSettings<T>(linkerData ?? new Dictionary<string, string>());
        case "IGlobalSettings":
          data.TryGetValue(GlobalStep.ID, out var globalData);
          return m_settingsCreator.CreateSettings<T>(globalData ?? new Dictionary<string, string>());
        default:
          throw new NotSupportedException();
      }
    }

    /// <inheritdoc />
    public (IMarkDocLogger logger, IReadOnlyCollection<IProcess> processes, Func<CancellationToken, ValueTask> executor) GenerateExecutor(IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> configuration)
    {
      var globalSettings = GetSettings<IGlobalSettings>(configuration);
      var linkerSettings = (LinkerSettings)GetSettings<ILinkerSettings>(configuration);
      var memberSettings = GetSettings<IMemberSettings>(configuration);
      var docSettings = GetSettings<IDocSettings>(configuration);
      var logger = TypeResolver.Resolve<IMarkDocLogger>();

      var memberProcess = new DefiniteProcess("Assembly resolver", memberSettings.Paths.Count);
      var documentationProcess = new DefiniteProcess("Documentation resolver", docSettings.Paths.Count);
      var printerProcess = new IndefiniteProcess("Printer");

      var processes = new IProcess[] { memberProcess, documentationProcess, printerProcess };

      return (logger, processes, async _ =>
      {
        var resolver = new Resolver(logger, memberProcess);
        var docResolver = new DocResolver(resolver, docSettings, logger, documentationProcess);

        await Task.WhenAll(resolver.ResolveAsync(memberSettings, globalSettings), docResolver.ResolveAsync())
          .ConfigureAwait(false);

        var linker = new Linker(resolver, linkerSettings);
        var diagrams = linkerSettings.Platform.Equals("3", StringComparison.InvariantCultureIgnoreCase)
          ? new EmptyDiagramResolver()
          : new MermaidResolver(linker) as IDiagramResolver;
        var composer = new TypeComposer(new Creator(false, linker.GetRawUrl()), docResolver, resolver, linker, diagrams);
        var printer = new PrinterMarkdown(composer, linker, printerProcess);

        await printer.Print(resolver.Types.Value.Values.SelectMany(Linq.XtoX), globalSettings.OutputPath)
          .ConfigureAwait(false);
      });
    }

    private sealed class EmptyDiagramResolver
      : IDiagramResolver
    {
      /// <inheritdoc />
      public bool TryGenerateDiagram(IType type, out (string name, string content) diagram)
      {
        diagram = (string.Empty, string.Empty);
        return false;
      }
    }

    internal sealed class SettingsCreator
      : ISettingsCreator
    {
      /// <inheritdoc />
      // ReSharper disable once AnnotateNotNullTypeMember
      public T CreateSettings<T>(IReadOnlyDictionary<string, string> data)
        where T : ILibrarySettings
        => typeof(T).Name switch
        {
          "IMemberSettings" => new MemberSettings(data) as dynamic,
          "IDocSettings" => new DocSettings(data),
          "ILinkerSettings" => new LinkerSettings(data),
          "IGlobalSettings" => new GlobalSettings(data),
          _ => throw new NotSupportedException(nameof(T))
        };
    }

    internal sealed class GlobalSettings
      : IGlobalSettings
    {
      #region Properties

      /// <inheritdoc />
      public Guid Id => Guid.Empty;

      /// <inheritdoc />
      public IReadOnlyCollection<string> IgnoredNamespaces { get; } = Array.Empty<string>();

      /// <inheritdoc />
      public IReadOnlyCollection<string> IgnoredTypes { get; } = Array.Empty<string>();

      /// <inheritdoc />
      public IReadOnlyCollection<string> CheckedIgnoredNamespaces { get; } = Array.Empty<string>();

      /// <inheritdoc />
      public IReadOnlyCollection<string> CheckedIgnoredTypes { get; } = Array.Empty<string>();

      /// <inheritdoc />
      public string OutputPath { get; } = string.Empty;

      #endregion

      /// <summary>
      /// Default constructor
      /// </summary>
      [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
      [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
      public GlobalSettings(IReadOnlyDictionary<string, string> data)
      {
        if (data.TryGetValue(nameof(OutputPath), out var outputPath))
          OutputPath = outputPath;
        if (data.TryGetValue(nameof(IgnoredNamespaces), out var ignoredNamespaces))
          IgnoredNamespaces = ignoredNamespaces.Split(IGlobalSettings.DELIM);
        if (data.TryGetValue(nameof(CheckedIgnoredNamespaces), out var checkedIgnoredNamespaces))
          CheckedIgnoredNamespaces = checkedIgnoredNamespaces.Split(IGlobalSettings.DELIM);
        if (data.TryGetValue(nameof(IgnoredTypes), out var ignoredTypes))
          IgnoredTypes = ignoredTypes.Split(IGlobalSettings.DELIM);
        if (data.TryGetValue(nameof(CheckedIgnoredTypes), out var checkedIgnoredTypes))
          CheckedIgnoredTypes = checkedIgnoredTypes.Split(IGlobalSettings.DELIM);
      }
    }

    internal sealed class Creator : IElementCreator
    {
      private readonly bool m_externalDiagrams;
      private readonly string m_rawUrl;

      /// <summary>
      /// Default constructor
      /// </summary>
      public Creator(bool externalDiagrams, string rawUrl)
      {
        m_externalDiagrams = externalDiagrams;
        m_rawUrl = rawUrl;
      }

      /// <inheritdoc />
      public IDiagram CreateDiagram(string name, string content)
        => new Diagram(name, "mermaid", content, m_externalDiagrams, m_rawUrl);

      public ILink CreateLink(IText content, Lazy<string> reference)
        => new Link(content, reference);

      public IList CreateList(
        IEnumerable<IElement> elements,
        IList.ListType type,
        string heading = "",
        int level = 0)
        => new ListElement(elements, type, heading, level);

      public IPage CreatePage(
        IEnumerable<IPage>? subpages = null,
        IEnumerable<IElement>? content = null,
        string heading = "",
        int level = 0)
        => new Page(this, content ?? Enumerable.Empty<IElement>(), subpages ?? Enumerable.Empty<IPage>(), heading, level);

      public ISection CreateSection(
        IEnumerable<IElement> content,
        string heading = "",
        int level = 0)
        => new Section(content, heading, level);

      public ITable CreateTable(
        IEnumerable<IReadOnlyCollection<IElement>> content,
        IEnumerable<IText> headings,
        string heading = "",
        int level = 0)
        => new Table(headings, content, heading, level);

      public IText CreateText(string content, IText.TextStyle style = IText.TextStyle.Normal)
        => new TextElement(content, style);

      public ITextContent JoinTextContent(
        IEnumerable<ITextContent> content,
        string delimiter)
        => new TextBuilder(content, delimiter);
    }
  }
}