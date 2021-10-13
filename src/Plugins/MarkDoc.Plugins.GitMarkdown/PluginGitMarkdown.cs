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
using MarkDoc.Printer;
using MarkDoc.Printer.Markdown;
using Module = Autofac.Module;

namespace MarkDoc.Plugins.GitMarkdown
{
  public sealed class PluginGitMarkdown
    : Module, IPlugin
  {
    private static readonly IContainer CONTAINER;

    #region Properties

    public string Id => "34C7AA14-E6FE-4684-BBE3-03C00F567297";

    public string Name => "Markdown for Git";

    public string Description => "Markdown documentation generating plugin for GitHub and GitLab";

    public Stream? Image => Assembly.GetExecutingAssembly().GetManifestResourceStream("MarkDoc.Plugin.GitMarkdown.icon.png");

    #endregion

    static PluginGitMarkdown()
    {
      var builder = new ContainerBuilder();
      builder.RegisterType<Resolver>().As<IResolver>().SingleInstance();
      builder.RegisterType<DocResolver>().As<IDocResolver>().SingleInstance();
      builder.RegisterType<Creator>().As<IElementCreator>().SingleInstance();
      builder.RegisterType<Linker>().As<ILinker>().SingleInstance();
      builder.RegisterType<TypeComposer>().As<ITypeComposer>().SingleInstance();
      builder.RegisterType<PrinterMarkdown>().As<IPrinter>().SingleInstance();
      builder.RegisterType<AssembliesStep>().As<IPluginStep>();
      builder.RegisterType<DocumentationStep>().As<IPluginStep>();
      builder.RegisterType<LinkerStep>().As<IPluginStep>();
      CONTAINER = builder.Build();
    }

    protected override void Load(ContainerBuilder builder) => builder.RegisterType<PluginGitMarkdown>().As<IPlugin>();

    public IReadOnlyCollection<IPluginStep> GetPluginSteps() => CONTAINER
      .Resolve<IEnumerable<IPluginStep>>()
      .OrderBy(step => step.StepNumber)
      .ToReadOnlyCollection();

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