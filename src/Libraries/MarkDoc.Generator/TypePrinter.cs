using MarkDoc.Documentation;
using MarkDoc.Elements;
using MarkDoc.Helpers;
using MarkDoc.Linkers;
using MarkDoc.Members;
using MarkDoc.Members.Enums;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MarkDoc.Generator
{
  public class TypePrinter
  {
    #region Fields

    private readonly IElementCreator m_creator;
    private readonly IDocResolver m_resolver;
    private readonly ILinker m_linker;
    private readonly ILogger m_logger;

    #endregion

    public TypePrinter(IElementCreator creator, IDocResolver resolver, ILinker linker, ILogger logger)
    {
      m_creator = creator;
      m_resolver = resolver;
      m_linker = linker;
      m_logger = logger;
    }

    public IPage Print(IInterface type)
    {
      if (type == null)
        throw new ArgumentNullException(nameof(type));

      var cache = CacheMemberDocs(type);

      if (!m_resolver.TryFindType(type, out var typeDoc, out var memberDoc))
        m_logger.LogWarning($"Missing documentation for {type.Name}");

      var page = m_creator.CreatePage();
      page.Heading = type.Name;
      page.Level = 0;

      var memberSection = PrintMemberTables(type, memberDoc ?? new Dictionary<string, IDocElement>());
      page.Content = memberSection.ToReadOnlyCollection();

      return page;
    }

    private IEnumerable<IElement> PrintMemberTables(IInterface type, IReadOnlyDictionary<string, IDocElement> memberDoc)
    {
      static IEnumerable<IGrouping<bool, IEnumerable<IGrouping<AccessorType, T>>>> GroupMembers<T>(IEnumerable<T> members)
        where T : IMember
        => members.GroupBy(x => !x.IsStatic).GroupBy(Linq.GroupKey, x => x.GroupBy(y => y.Accessor));

      IElement CreateMethodSection(IEnumerable<IMethod> methods, bool isStatis, AccessorType access)
      {
        static (bool hasOverloads, IMethod method) IsolateOverloads(IGrouping<string, IMethod> input)
        {
          var items = input.Take(2).ToArray();

          return (items.Length == 1, items.First());
        }

        IReadOnlyCollection<IElement> CreateRow((bool hasOverloads, IMethod method) input)
        {
          static string ProcessArguments(IArgument argument)
          {
            var keyword = argument.Keyword switch
            {
              ArgumentType.Normal => string.Empty,
              ArgumentType.Ref => "ref ",
              ArgumentType.Out => "out ",
              ArgumentType.In => "in ",
              _ => "",
            };
            var type = argument.Type.DisplayName;
            var name = argument.Name;

            return $"{keyword}{type} {name}";
          }

          IElement ProcessReturn()
          {
            var content = m_creator.CreateText(input.method.Returns?.DisplayName ?? "void", IText.TextStyle.CodeInline);

            if (input.method.Returns == null)
              return content;

            var link = m_linker.CreateLink(input.method.Returns);
            if (string.IsNullOrEmpty(link))
              return content;

            return m_creator.CreateLink(content, link);
          }

          IElement ProcessMethod()
          {
            var method = m_creator.CreateText($"{input.method.Name}({(input.hasOverloads ? "..." : string.Join(", ", input.method.Arguments.Select(ProcessArguments)))})", IText.TextStyle.CodeInline);

            return method;
          }

          var returns = ProcessReturn();
          var method = ProcessMethod();

          return new[] { returns, method };
        }

        var table = m_creator.CreateTable();
        var grouped = methods.GroupBy(x => x.Name)
                             .OrderBy(Linq.GroupKey)
                             .Select(IsolateOverloads)
                             .Select(CreateRow)
                             .ToReadOnlyCollection();
        table.Headings = new[] { m_creator.CreateText("Returns"), m_creator.CreateText("Name") };
        table.Content = grouped;

        return table;
      }

      IElement CreatePropertySection(IEnumerable<IProperty> properties, bool isStatis, AccessorType access)
      {
        var table = m_creator.CreateTable();

        return table;
      }

      static IEnumerable<IElement> ProcessMembers<T>(Func<IEnumerable<T>, bool, AccessorType, IElement> processor, IEnumerable<IGrouping<bool, IEnumerable<IGrouping<AccessorType, T>>>> members)
        where T : IMember
      {
        foreach (var statics in members)
          foreach (var access in statics.SelectMany(Linq.XtoX))
            yield return processor(access.Select(Linq.XtoX), statics.Key, access.Key);
      }

      var methods = ProcessMembers(CreateMethodSection, GroupMembers(type.Methods));
      var properties = ProcessMembers(CreatePropertySection, GroupMembers(type.Properties));

      return methods.Concat(properties);
    }

    private IEnumerable<ITag> FindTag(IType type, ITag.TagType tag)
    {
      IEnumerable<ITag> ProcessClass(IClass classDef)
      {
        m_resolver.TryFindType(classDef, out var typeDoc, out var _);
        if (typeDoc == null)
          return Enumerable.Empty<ITag>();
        if (!typeDoc.Documentation.Tags.TryGetValue(tag, out var result))
        {
          if (!typeDoc.Documentation.HasInheritDoc || classDef.BaseClass == null)
            return Enumerable.Empty<ITag>();

          var baseType = classDef.BaseClass.Reference.Value;
          if (baseType != null && baseType is IClass baseDef)
            return ProcessClass(baseDef);

          return Enumerable.Empty<ITag>();
        }

        return result;
      }

      IEnumerable<ITag> ProcessType(IType typeDef)
      {
        m_resolver.TryFindType(typeDef, out var typeDoc, out var _);
        if (typeDoc == null)
          return Enumerable.Empty<ITag>();

        if (!typeDoc.Documentation.Tags.TryGetValue(tag, out var result))
          return Enumerable.Empty<ITag>();

        return result;
      }

      return type switch
      {
        IClass classDef => ProcessClass(classDef),
        _ => ProcessType(type),
      };
    }

    private IEnumerable<ITag> FindTag(IType type, IMember member, ITag.TagType tag)
    {
      IEnumerable<ITag> ProcessInterface(IInterface interfaceDef)
      {
        IEnumerable<ITag> Process(string rawName)
        {
          m_resolver.TryFindType(interfaceDef, out var _, out var memberDocs);
          if (memberDocs == null)
            return Enumerable.Empty<ITag>();

          if (!memberDocs.TryGetValue(member.RawName, out var memberDoc))
            return Enumerable.Empty<ITag>();

          if (!memberDoc.Documentation.Tags.TryGetValue(tag, out var result))
          {
            if (!memberDoc.Documentation.HasInheritDoc)
              return Enumerable.Empty<ITag>();

            var inheritDoc = memberDoc.Documentation.Tags[ITag.TagType.Inheritdoc].First();
            if (!string.IsNullOrEmpty(inheritDoc.Reference))
              return Process(inheritDoc.Reference);
          }

          return result;
        }

        return Process(interfaceDef.RawName);
      }

      IEnumerable<ITag> ProcessType(IType typeDef)
      {
        m_resolver.TryFindType(typeDef, out var _, out var memberDocs);
        if (memberDocs == null)
          return Enumerable.Empty<ITag>();

        if (!memberDocs.TryGetValue(member.RawName, out var memberDoc))
          return Enumerable.Empty<ITag>();

        if (!memberDoc.Documentation.Tags.TryGetValue(tag, out var result))
          return Enumerable.Empty<ITag>();

        return result;
      }

      return type switch
      {
        IClass classDef
          => ProcessInterface(classDef),
        IInterface interfaceDef
          => ProcessInterface(interfaceDef),
        _ => ProcessType(type)
      };
    }

    private IReadOnlyDictionary<string, IReadOnlyDictionary<ITag.TagType, IReadOnlyCollection<ITag>>> CacheMemberDocs(IInterface type)
    {
      if (!m_resolver.TryFindType(type, out var _, out var memberDocs) || memberDocs == null)
        return new Dictionary<string, IReadOnlyDictionary<ITag.TagType, IReadOnlyCollection<ITag>>>();

      void Process(Dictionary<string, Dictionary<ITag.TagType, IReadOnlyCollection<ITag>>> cache, string[] names)
      {
        var references = new List<string>();
        foreach (var name in names)
        {
          var except = new HashSet<ITag.TagType>(cache[name].Select(x => x.Key));
          var tags = memberDocs[name].Documentation.Tags.Where(x => x.Key != ITag.TagType.Inheritdoc && !except.Contains(x.Key));
          foreach (var tag in tags)
            cache[name].Add(tag.Key, tag.Value);

          if (!string.IsNullOrEmpty(memberDocs[name].Documentation.InheritDocRef))
            references.Add(name);
        }

        var baseClass = (type is IClass classDef && classDef.BaseClass?.Reference.Value != null)
          ? new[] { classDef.BaseClass.Reference.Value }
          : Enumerable.Empty<IType>();

        var sources = type.InheritedInterfaces
          .Select(x => x.Reference.Value)
          .WhereNotNull()
          .Concat(baseClass)
          .OfType<IInterface>()
          .ToDictionary(x => x.RawName);

        foreach (var source in sources)
        {

        }
      }

      var result = new Dictionary<string, IReadOnlyDictionary<ITag.TagType, IReadOnlyCollection<ITag>>>(memberDocs.Count);

      foreach (var item in memberDocs.GroupBy(x => x.Value.Documentation.HasInheritDoc))
      {
        if (item.Key)
        {
          var temps = new Dictionary<string, Dictionary<ITag.TagType, IReadOnlyCollection<ITag>>>(item.ToDictionary(x => x.Key, x => new Dictionary<ITag.TagType, IReadOnlyCollection<ITag>>()));
          Process(temps, temps.Select(x => x.Key).ToArray());

          // Cache collected documentation
          foreach (var temp in temps)
            result.Add(temp.Key, temp.Value);
        }
        else
          foreach (var member in item.Select(Linq.XtoX))
            result.Add(member.Key, member.Value.Documentation.Tags);
      }

      return result;
    }
  }
}
