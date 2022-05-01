using System;
using System.Collections.Generic;
using System.Linq;
using MarkDoc.Members.ResolvedTypes;
using MarkDoc.Members.Types;

namespace MarkDoc.Diagrams.PlantUML
{
  public class PlantUmlResolver
    : IDiagramResolver
  {
    /// <inheritdoc />
    public bool TryGenerateDiagram(IType type, out (string name, string content) diagram)
    {
      var types = new Dictionary<string, LinkedList<string>>(StringComparer.OrdinalIgnoreCase);
      var relations = new LinkedList<string>();

      // ReSharper disable once VariableHidesOuterVariable
      (string nameSpace, string typeDiagram) GenerateType(IType type, bool isParent = false)
      {
        string GenerateTitle(IInterface? inter = default)
        {
          var generics = inter is not null && inter.Generics.Keys.Any()
            ? $"<{string.Join(",", inter.Generics.Keys)}>"
            : string.Empty;

          var highlightedTitle = isParent
            ? $" as \"**{type.Name}**{generics}\""
            : $" as \"{type.Name}{generics}\"";

          return highlightedTitle;
        }

        var rawTitle = type.RawName.Replace('`', '_');
        var typeName = type switch
        {
          IRecord r => $"class {rawTitle}{GenerateTitle(r)} << (R, orchid) >>",
          IClass c => $"{(c.IsAbstract ? "abstract " : "")}class {rawTitle}{GenerateTitle(c)}",
          IInterface i => $"interface {rawTitle}{GenerateTitle(i)}",
          IEnum => $"enum {rawTitle}{GenerateTitle()}",
          _ => throw new NotSupportedException("The given type is not supported")
        };

        return (type.TypeNamespace, typeName);
      }

      // ReSharper disable once VariableHidesOuterVariable
      (string nameSpace, string typeDiagram) GenerateResType(IResType type)
        => type.Reference.Value is not null
          // ReSharper disable once AssignNullToNotNullAttribute
          ? GenerateType(type.Reference.Value)
          : (type.TypeNamespace, $"{type.RawName.Replace('`', '_')} << External >>");

      void ExtractTypes(IType parent, bool isParent)
      {
        AddToDictionary(types, GenerateType(parent, isParent));

        if (parent is not IInterface interfaceType)
          return;

        foreach (var item in interfaceType.InheritedInterfaces)
        {
          relations.AddLast($"{parent.RawName.Replace('`', '_')} <-- {item.RawName.Replace('`', '_')}");
          AddToDictionary(types, GenerateResType(item));
        }

        if (parent is not IClass classType || classType.BaseClass is null)
          return;

        var baseType = GenerateResType(classType.BaseClass!);

        relations.AddLast($"{parent.RawName.Replace('`', '_')} <-- {classType.BaseClass!.RawName.Replace('`', '_')}");
        AddToDictionary(types, baseType);
      }

      ExtractTypes(type, true);

      diagram = (type.RawName.Replace('`', '_'), string.Join(Environment.NewLine, PackTypes(types).Concat(relations)));
      return true;
    }

    private static IEnumerable<string> PackTypes(IReadOnlyDictionary<string, LinkedList<string>> types)
      => types.Select(type => $"package {type.Key} <<Rectangle>> {{ {Environment.NewLine} { string.Join(Environment.NewLine, type.Value) } {Environment.NewLine} }}").ToList();

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
