using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using ModularDoc.Linkers;
using ModularDoc.Members.ResolvedTypes;
using ModularDoc.Members.Types;

namespace ModularDoc.Diagrams.Mermaid
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

    private static string Normalize(string value)
    {
      var span = value.AsSpan();
      var index = span.IndexOf('<');
      span = index == -1
        ? span
        : span[..index];

      var chars = span.ToArray();
      for (var i = 0; i < chars.Length; i++)
      {
        if (chars[i] == '`')
          chars[i] = '_';
      }

      return new string(chars);
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

        var rawTitle = Normalize(type.RawName);
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

              builder.AppendLine($"{Normalize(constraint.RawName)} --> {rawTitle}{key}");
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
          : (type.TypeNamespace, $"{Normalize(type.RawName)}[[{type.DisplayName}]]");

      void ExtractTypes(IType parent, bool isParent)
      {
        var parentRawName = Normalize(parent.RawName);
        AddToDictionary(types, GenerateType(parent, isParent));
        typeNodes.Add(Normalize(parentRawName));

        if (parent is not IInterface interfaceType)
          return;

        void ProcessInheritance(string parentName, IEnumerable<TreeNode> source)
        {
          foreach (var item in source)
          {
            var itemName = Normalize(item.Name);

            relations.AddLast($"{itemName} --> {parentName}");
            AddToDictionary(types, GenerateResType(item.Value));
            typeNodes.Add(itemName);

            ProcessInheritance(itemName, item.Children);
          }
        }

        ProcessInheritance(parentRawName, interfaceType.InheritedTypesStructured.Value);

        if (parent is not IClass classType || classType.BaseClass is null)
          return;

        var baseType = GenerateResType(classType.BaseClass!);
        var baseRawName = Normalize(classType.BaseClass!.RawName);

        relations.AddLast($"{Normalize(baseRawName)} --> {parentRawName}");
        AddToDictionary(types, baseType);
        typeNodes.Add(baseRawName);
      }

      ExtractTypes(type, true);

      diagram = (Normalize(type.RawName), $"flowchart LR{Environment.NewLine}{STYLES}{Environment.NewLine}{string.Join(Environment.NewLine, PackTypes(types).Concat(relations))}");
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