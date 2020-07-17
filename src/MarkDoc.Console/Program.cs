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
using MarkDoc.Members.Types;

namespace MarkDoc.Console
{
  public static class Program
  {
    internal sealed class Creator
      : IElementCreator
    {
      public ILink CreateLink(IText content, string reference = "")
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

    private const string OUTPUT = @"";

    private static readonly IReadOnlyCollection<string> DLL_PATHS = new[]
    {
      ""
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
      builder.RegisterType<TypePrinter>().As<ITypePrinter>().SingleInstance();

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
      var printer = container.Resolve<ITypePrinter>();

      var linker = container.Resolve<ILinker>();

      async Task Print(IElement element, IType type)
      {
        var path = Path.Combine(OUTPUT, linker.Paths[type]) + ".md";

        if (!Directory.Exists(path))
          Directory.CreateDirectory(Path.GetDirectoryName(path));

        await using var file = File.CreateText(path);
        await file.WriteAsync(element.ToString()).ConfigureAwait(false);
      }

      var pages = result.Values
        .SelectMany(Linq.XtoX)
        .Select(x => Print(printer.Print(x), x));

      await Task.WhenAll(pages).ConfigureAwait(false);

      await container.Disposer.DisposeAsync().ConfigureAwait(false);
    }
  }
}
