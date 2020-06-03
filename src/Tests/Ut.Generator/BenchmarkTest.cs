using Autofac;
using MarkDoc.Documentation;
using MarkDoc.Documentation.Xml;
using MarkDoc.Elements;
using MarkDoc.Generator;
using MarkDoc.Helpers;
using MarkDoc.Linkers;
using MarkDoc.Members;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarkDoc.Members.ResolvedTypes;
using MarkDoc.Elements.Markdown;
using NUnit.Framework;
using Table = MarkDoc.Elements.Markdown.Table;
using MarkDoc.Members.Dnlib;
using MarkDoc.Generator.Basic;

namespace Ut.Generator
{
  [SingleThreaded]
  public class BenchmarkTest
    : IDisposable
  {
    private readonly ConcurrentBag<long> m_ticks;
    private readonly IReadOnlyCollection<string> m_xmlPaths;
    private readonly IReadOnlyCollection<string> m_dllPaths;

    public BenchmarkTest()
    {
      m_ticks = new ConcurrentBag<long>();
      m_dllPaths = new[]
      {
        ""
      };
      m_xmlPaths = new[]
      {
        ""
      };
    }

    [Test]
    [Repeat(100)]
    public async Task Bench()
    {
      var builder = new ContainerBuilder();
      builder.RegisterType<Resolver>().As<IResolver>().SingleInstance();
      builder.RegisterType<DocResolver>().As<IDocResolver>().SingleInstance();
      builder.RegisterType<Creator>().As<IElementCreator>().SingleInstance();
      builder.RegisterType<Linker>().As<ILinker>().SingleInstance();
      builder.RegisterType<TypePrinter>().As<ITypePrinter>().SingleInstance();
      var container = builder.Build();

      var resolver = container.Resolve<IResolver>();
      var docResolver = container.Resolve<IDocResolver>();

      var watch = new Stopwatch();
      watch.Start();

      var tasks = m_xmlPaths.Select(docResolver.Resolve)
                          .Concat(new[] { Task.Run(() => Parallel.ForEach(m_dllPaths, resolver.Resolve)) });

      await Task.WhenAll(tasks).ConfigureAwait(false);

      var result = resolver.Types.Value;
      var printer = container.Resolve<ITypePrinter>();

      var bag = new ConcurrentBag<IPage>();
      var pages = result.Values
        .AsParallel()
        .SelectMany(Linq.XtoX)
        .OfType<IInterface>()
        .Select(printer.Print);
      pages.ForAll(bag.Add);

      watch.Stop();
      m_ticks.Add(watch.ElapsedTicks);
      await container.Disposer.DisposeAsync();
    }

    public async void Dispose()
    {
      await using var file = File.CreateText(@"C:\Users\hp\Desktop\output.txt");
      await file.WriteAsync(new StringBuilder().AppendJoin(Environment.NewLine, m_ticks)).ConfigureAwait(false);
    }
  }

  public class Linker
    : ILinker
  {
    public string CreateLink(IResType type)
      => string.Empty;
  }

  public class Creator
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
}
