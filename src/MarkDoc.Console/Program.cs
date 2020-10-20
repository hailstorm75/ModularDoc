using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
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

namespace MarkDoc.Console
{
  public static class Program
  {
#pragma warning disable CA1812
    internal sealed class Creator
      : IElementCreator
    {
      public ILink CreateLink(IText content, Lazy<string> reference)
        => new Link(content, reference);

      public IList CreateList(IEnumerable<IElement> elements, IList.ListType type, string heading = "", int level = 0)
        => new ListElement(elements, type, heading, level);

      public IPage CreatePage(IEnumerable<IPage>? subpages = null, IEnumerable<IElement>? content = null, string heading = "", int level = 0)
        => new Page(this, content ?? Enumerable.Empty<IElement>(), subpages ?? Enumerable.Empty<IPage>(), heading, level);

      public ISection CreateSection(IEnumerable<IElement> content, string heading = "", int level = 0)
        => new Section(content, heading, level);

      public ITable CreateTable(IEnumerable<IReadOnlyCollection<IElement>> content, IEnumerable<IText> headings, string heading = "", int level = 0)
        => new Table(headings, content, heading, level);

      public IText CreateText(string content, IText.TextStyle style = IText.TextStyle.Normal)
        => new TextElement(content, style);

      public ITextContent JoinTextContent(IEnumerable<ITextContent> content, string delimiter)
        => new TextBuilder(content, delimiter);
    }

    #region Fields

    private const string OUTPUT = @"C:\Users\Denis\Desktop\MarkDoc";

    private static readonly IReadOnlyCollection<string> DLL_PATHS = new[]
    {
      @"C:\Users\Denis\Source\Repos\homework\ClassLibrary1\ClassLibrary1\bin\Debug\netcoreapp3.1\ClassLibrary1.dll"
    };

    private static readonly IReadOnlyCollection<string> XML_PATHS = DLL_PATHS.Select(x => Path.Join(Path.GetDirectoryName(x), Path.GetFileNameWithoutExtension(x) + ".xml")).ToArray();

    #endregion

    private static IContainer Build()
    {
      var builder = new ContainerBuilder();
      builder.RegisterType<Resolver>().As<IResolver>().SingleInstance();
      builder.RegisterType<DocResolver>().As<IDocResolver>().SingleInstance();
      builder.RegisterType<Creator>().As<IElementCreator>().SingleInstance();
      builder.RegisterType<Linker>().As<ILinker>().SingleInstance();
      builder.RegisterType<TypeComposer>().As<ITypeComposer>().SingleInstance();
      builder.RegisterType<PrinterMarkdown>().As<IPrinter>().SingleInstance();

      return builder.Build();
    }

    private static async Task Main()
      => await Run().ConfigureAwait(false);

    private static async Task Run()
    {
      var container = Build();

      var resolver = container.Resolve<IResolver>();
      var docResolver = container.Resolve<IDocResolver>();

      var tasks = XML_PATHS
        .Select(docResolver.Resolve)
        .Concat(new[] { Task.Run(() => Parallel.ForEach(DLL_PATHS, resolver.Resolve)) });

      await Task.WhenAll(tasks).ConfigureAwait(false);

      var result = resolver.Types.Value;

      var printer = container.Resolve<IPrinter>();
      await printer.Print(result.Values.SelectMany(Linq.XtoX), OUTPUT).ConfigureAwait(false);

      await container.Disposer.DisposeAsync().ConfigureAwait(false);
    }
  }
}
