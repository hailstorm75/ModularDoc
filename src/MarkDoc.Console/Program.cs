using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System;
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
using MarkDoc.Members;
using MarkDoc.Members.Dnlib;
using MarkDoc.Members.ResolvedTypes;
using NBench;

namespace MarkDoc.Console
{
  public class Bench
  {
    public class Linker
      : ILinker
    {
      public string CreateLink(IResType type)
        => "google.com";
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

    #region Fields

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

    [PerfBenchmark(NumberOfIterations = 3, RunMode = RunMode.Iterations, TestMode = TestMode.Measurement)]
    [TimingMeasurement]
    [MemoryMeasurement(MemoryMetric.TotalBytesAllocated)]
    public void Foo()
    {
      var container = Build();

      var resolver = container.Resolve<IResolver>();
      var docResolver = container.Resolve<IDocResolver>();

      var tasks = XML_PATHS.Select(docResolver.Resolve)
        .Concat(new[] { Task.Run(() => Parallel.ForEach(DLL_PATHS, resolver.Resolve)) });

      Task.WhenAll(tasks).GetAwaiter().GetResult();

      var result = resolver.Types.Value;
      var printer = container.Resolve<ITypePrinter>();

      var bag = new ConcurrentBag<string>();
      var pages = result.Values
        .AsParallel()
        .SelectMany(Linq.XtoX)
        .Select(printer.Print)
        .Select(x => x.ToString());
      pages.ForAll(bag.Add);

      container.Disposer.Dispose();
    }

    [PerfBenchmark(NumberOfIterations = 3, RunMode = RunMode.Iterations, TestMode = TestMode.Measurement)]
    [TimingMeasurement]
    [MemoryMeasurement(MemoryMetric.TotalBytesAllocated)]
    public void Bar()
    {
      var container = Build();

      var resolver = container.Resolve<IResolver>();
      var docResolver = container.Resolve<IDocResolver>();

      var tasks = XML_PATHS.Select(docResolver.Resolve)
        .Concat(new[] { Task.Run(() => Parallel.ForEach(DLL_PATHS, resolver.Resolve)) });

      Task.WhenAll(tasks).GetAwaiter().GetResult();

      var result = resolver.Types.Value;
      var printer = container.Resolve<ITypePrinter>();

      var pages = result.Values
        .SelectMany(Linq.XtoX)
        .Select(printer.Print)
        .Select(x => x.ToString())
        .ToReadOnlyCollection();

      container.Disposer.Dispose();
    }
  }

  public static class Program
  {
    private static void Main()
    {
      NBenchRunner.Run<Bench>();
      System.Console.ReadLine();
    }
  }
}
