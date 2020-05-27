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
using MarkDoc.Documentation.Tags;

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

      var page = m_creator.CreatePage();
      page.Heading = type.Name;
      page.Level = 0;

      var memberSection = PrintMemberTables(type);
      page.Content = memberSection.ToReadOnlyCollection();

      return page;
    }

    private IEnumerable<IElement> PrintMemberTables(IInterface type)
    {
      static IEnumerable<IGrouping<bool, IEnumerable<IGrouping<AccessorType, T>>>> GroupMembers<T>(IEnumerable<T> members)
        where T : IMember
        => members.GroupBy(x => !x.IsStatic).GroupBy(Linq.GroupKey, x => x.GroupBy(y => y.Accessor));

      IElement CreateMethodSection(IEnumerable<IMethod> methods, bool isStatis, AccessorType access)
      {
        static (bool hasOverloads, IMethod method) IsolateOverloads(IGrouping<string, IMethod> input)
        {
          // Bug: Identify as overloads only if equally named members are on the same access level
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
            var method = $"{input.method.Name}({(input.hasOverloads ? "..." : string.Join(", ", input.method.Arguments.Select(ProcessArguments)))})";
            var summary = FindTag(type, input.method, ITag.TagType.Summary).FirstOrDefault();

            return m_creator.CreateText(method, IText.TextStyle.CodeInline);
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
        m_resolver.TryFindType(classDef, out var typeDoc);
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
        m_resolver.TryFindType(typeDef, out var typeDoc);
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
      IEnumerable<ITag> ProcessType(IType typeDef)
      {
        m_resolver.TryFindType(typeDef, out var typeDoc);
        if (typeDoc == null)
          return Enumerable.Empty<ITag>();

        if (!typeDoc.Members.Value.TryGetValue(member.RawName, out var memberDoc))
          return Enumerable.Empty<ITag>();

        if (!memberDoc.Documentation.Tags.TryGetValue(tag, out var result))
          return Enumerable.Empty<ITag>();

        return result;
      }

      return ProcessType(type);
    }
  }
}
