using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Autofac;
using MarkDoc.Core;
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
    #region Properties

    /// <inheritdoc />
    public string Id => "34C7AA14-E6FE-4684-BBE3-03C00F567297";

    /// <inheritdoc />
    public string Name => "Markdown for Git";

    /// <inheritdoc />
    public string Description => "Markdown documentation generating plugin for GitHub and GitLab";

    /// <inheritdoc />
    public Stream? Image => Assembly.GetExecutingAssembly().GetManifestResourceStream("MarkDoc.Plugin.GitMarkdown.icon.png");

    #endregion

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
    }

    /// <inheritdoc />
    public IReadOnlyCollection<IPluginStep> GetPluginSteps() => TypeResolver
      .Resolve<IEnumerable<IPluginStep>>()
      .OrderBy(step => step.StepNumber)
      .ToReadOnlyCollection();

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
          // "IDocSettings" => new DocSettings(data),
          "ILinkerSettings" => new LinkerSettings(data),
          _ => throw new NotSupportedException(nameof(T))
        };
    }

    internal sealed class Creator : IElementCreator
    {
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