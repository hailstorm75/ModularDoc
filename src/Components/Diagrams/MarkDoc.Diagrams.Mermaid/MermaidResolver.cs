using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using MarkDoc.Linkers;
using MarkDoc.Members.ResolvedTypes;
using MarkDoc.Members.Types;

namespace MarkDoc.Diagrams.Mermaid
{
  public class MermaidResolver
    : IDiagramResolver
  {
    private readonly ILinker m_linker;

    private const string STYLES = @"  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px";

    /// <summary>
    /// Default constructor
    /// </summary>
    public MermaidResolver(ILinker linker)
    {
      m_linker = linker;
    }

    /// <inheritdoc />
    [SuppressMessage("ReSharper", "RedundantSuppressNullableWarningExpression")]
    public bool TryGenerateDiagram(IType type, out (string name, string content) diagram)
    {
      var types = new Dictionary<string, LinkedList<string>>(StringComparer.OrdinalIgnoreCase);
      var typeNodes = new HashSet<string>();
      var relations = new LinkedList<string>();

      // ReSharper disable once VariableHidesOuterVariable
      (string nameSpace, string typeDiagram) GenerateType(IType type, bool isParent = false)
      {
        string GenerateTitle(IInterface? inter = default)
        {
          var generics = inter is not null && inter.Generics.Keys.Any()
            ? $"< {string.Join(",", inter.Generics.Keys)} >"
            : string.Empty;

          var highlightedTitle = $"{type.Name}{generics}";

          return highlightedTitle;
        }

        var rawTitle = type.RawName.Replace('`', '_');
        var typeName = type switch
        {
          IRecord r => $"  {rawTitle}[[{GenerateTitle(r)}]]",
          IClass c => $"  {rawTitle}[[{GenerateTitle(c)}]]" + (c.IsAbstract ? $"{Environment.NewLine}  class {rawTitle} abstractStyle;" : string.Empty),
          IInterface i => $"  {rawTitle}[[{GenerateTitle(i)}]]{Environment.NewLine}  class {rawTitle} interfaceStyle;",
          IEnum => $"  {rawTitle}[[{GenerateTitle()}]]",
          _ => throw new NotSupportedException("The given type is not supported")
        };

        if (type is IInterface inter && inter.Generics.Keys.Any())
        {
          var builder = new StringBuilder();
          foreach (var (key, (_, constraints)) in inter.Generics)
          {
            if (!constraints.Any())
              continue;

            builder.AppendLine($"  {rawTitle}{key}(({key}));");
            builder.AppendLine($"  {rawTitle} -- where --o {rawTitle}{key}");

            foreach (var constraint in constraints)
            {
              if (!typeNodes!.Contains(constraint.RawName))
              {
                var res = GenerateResType(constraint);
                AddToDictionary(types!, res);
                typeNodes.Add(constraint.RawName);
              }

              builder.AppendLine($"{constraint.RawName.Replace('`', '_')} --> {rawTitle}{key}");
            }
          }

          typeName += $"{Environment.NewLine}{builder}";
        }

        // var link = m_linker.CreateLink(type);

        return (type.TypeNamespace, typeName);
      }

      // ReSharper disable once VariableHidesOuterVariable
      (string nameSpace, string typeDiagram) GenerateResType(IResType type)
        => type.Reference.Value is not null
          // ReSharper disable once AssignNullToNotNullAttribute
          ? GenerateType(type.Reference.Value)
          : (type.TypeNamespace, $"{type.RawName.Replace('`', '_')}[[{type.DisplayName}]]");

      void ExtractTypes(IType parent, bool isParent)
      {
        AddToDictionary(types, GenerateType(parent, isParent));
        typeNodes.Add(parent.RawName);

        if (parent is not IInterface interfaceType)
          return;

        foreach (var item in interfaceType.InheritedInterfaces)
        {
          relations.AddLast($"{item.RawName.Replace('`', '_')} --> {parent.RawName.Replace('`', '_')}");
          AddToDictionary(types, GenerateResType(item));
          typeNodes.Add(item.RawName);
        }

        if (parent is not IClass classType || classType.BaseClass is null)
          return;

        var baseType = GenerateResType(classType.BaseClass!);

        relations.AddLast($"{classType.BaseClass!.RawName.Replace('`', '_')} --> {parent.RawName.Replace('`', '_')}");
        AddToDictionary(types, baseType);
        typeNodes.Add(classType.BaseClass.RawName);
      }

      ExtractTypes(type, true);

      diagram = (type.RawName.Replace('`', '_'), $"flowchart LR{Environment.NewLine}{STYLES}{Environment.NewLine}{string.Join(Environment.NewLine, PackTypes(types).Concat(relations))}");
      return true;
    }

    private static IEnumerable<string> PackTypes(IReadOnlyDictionary<string, LinkedList<string>> types)
      => types.Select(type => $"  subgraph {type.Key}{Environment.NewLine}{string.Join(Environment.NewLine, type.Value)}{Environment.NewLine}  end");

    private static void AddToDictionary(IDictionary<string, LinkedList<string>> target, (string key, string value) data)
    {
      var (key, value) = data;
      if (target.ContainsKey(key))
        target[key].AddLast(value);
      else
      {
        var list = new LinkedList<string>();
        list.AddLast(value);
        target.Add(key, list);
      }
    }
  }
}