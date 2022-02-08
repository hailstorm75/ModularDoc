using MarkDoc.Members.ResolvedTypes;
using MarkDoc.Members.Types;

namespace MarkDoc.Diagrams.PlantUML
{
  public class PlantUMLResolver
    : IDiagramResolver
  {
    /// <inheritdoc />
    public bool TryGenerateDiagram(IType type, out string diagram)
    {
      var types = new Dictionary<string, LinkedList<string>>(StringComparer.OrdinalIgnoreCase);
      var relations = new LinkedList<string>();

      // ReSharper disable once VariableHidesOuterVariable
      (string nameSpace, string typeDiagram) GenerateType(IType type, bool isParent = false)
      {
        var highlightedTitle = isParent
          ? $" as \"**{type.Name}**\""
          : string.Empty;

        var typeName = type switch
        {
          IRecord => $"class {type.Name}{highlightedTitle} << (R, orchid) >>",
          IClass c => $"{(c.IsAbstract ? "abstract " : "")}class {type.Name}{highlightedTitle}",
          IInterface => $"interface {type.Name}{highlightedTitle}",
          IEnum => $"enum {type.Name}{highlightedTitle}",
          _ => throw new NotSupportedException("The given type is not supported")
        };

        ExtractTypes(type, false);

        return (type.TypeNamespace, typeName);
      }

      // ReSharper disable once VariableHidesOuterVariable
      (string nameSpace, string typeDiagram) GenerateResType(IResType type)
        => type.Reference.Value is not null
          // ReSharper disable once AssignNullToNotNullAttribute
          ? GenerateType(type.Reference.Value)
          : (type.TypeNamespace, $"{type.DisplayName} << External >>");

      void ExtractTypes(IType parent, bool isParent)
      {
        // ReSharper disable once AssignNullToNotNullAttribute
        AddToDictionary(types, GenerateType(type, isParent));

        if (parent is not IInterface interfaceType)
          return;

        foreach (var item in interfaceType.InheritedInterfaces)
        {
          relations.AddLast($"{parent.Name} <|-- {item.DisplayName}");
          AddToDictionary(types, GenerateResType(item));
        }

        if (parent is not IClass classType || classType.BaseClass is not null)
          return;

        var baseType = GenerateResType(classType.BaseClass!);

        relations.AddLast($"{parent.Name} <|-- {classType.BaseClass!.DisplayName}");
        AddToDictionary(types, baseType);
      }

      ExtractTypes(type, true);

      diagram = string.Join(Environment.NewLine, PackTypes(types).Concat(relations));
      return true;
    }

    private static IEnumerable<string> PackTypes(IReadOnlyDictionary<string, LinkedList<string>> types)
      => types.Select(type => string.Format(@"package {0} <<Rectangle>> {
  {1}
}", type.Key, string.Join(Environment.NewLine, type.Value)));

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
