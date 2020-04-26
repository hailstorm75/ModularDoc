using dnlib.DotNet;
using MarkDoc.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Linq;
using MarkDoc.Members.Dnlib.Properties;

namespace MarkDoc.Members.Dnlib
{
  public class Resolver
    : LazySingleton<Resolver>, IResolver
  {
    #region Fields

    private static readonly HashSet<string> EXCLUDED_NAMESPACES = new HashSet<string> { "System", "Microsoft" };
    private readonly LinkedList<IEnumerable<IGrouping<string, IReadOnlyCollection<IType>>>> m_groups;

    #endregion

    #region Properties

    public Lazy<IReadOnlyDictionary<string, IReadOnlyCollection<IType>>> Types { get; }

    #endregion

    public Resolver()
    {
      m_groups = new LinkedList<IEnumerable<IGrouping<string, IReadOnlyCollection<IType>>>>();
      Types = new Lazy<IReadOnlyDictionary<string, IReadOnlyCollection<IType>>>(ComposeTypes, LazyThreadSafetyMode.ExecutionAndPublication);
    }

    #region Methods

    public void Resolve(string assembly)
    {
      if (Types.IsValueCreated)
        throw new InvalidOperationException(Resources.resolveAfterMaterializeForbidden);
      if (!File.Exists(assembly))
        throw new FileNotFoundException(assembly);

      var module = ModuleDefMD.Load(assembly);
      var group = module.GetTypes()
                        .Where(x => !x.FullName.Equals("<Module>", StringComparison.InvariantCultureIgnoreCase))
                        .GroupBy(x => x.Namespace.String)
                        .Where(x => FilterNamespaces(x.Key))
                        .GroupBy(x => x.Key, x => x.Select(t => ResolveType(t)).SelectMany(x => x).ToArray() as IReadOnlyCollection<IType>);

      m_groups.AddLast(group);
    }

#pragma warning disable CA1822 // Mark members as static
    public IResType Resolve(dnlib.DotNet.TypeSig source)
    {
      if (source == null)
        throw new ArgumentNullException(nameof(source));

      return source.ElementType switch
      {
        ElementType.Boolean
          => new ResValueType(source, "bool"),
        ElementType.Char
          => new ResValueType(source, "char"),
        ElementType.String
          => new ResValueType(source, "string"),
        var x when x is ElementType.SZArray || x is ElementType.Array
          => new ResArray(source),
        var x when (x is ElementType.GenericInst || x is ElementType.MVar) && IsGeneric(source)
          => IsTuple(source)
              ? new ResTuple(source)
              : new ResGeneric(source) as IResType,
        ElementType.Object
          => new ResValueType(source, "object"),
        ElementType.I1
          => new ResValueType(source, "sbyte"),
        ElementType.U1
          => new ResValueType(source, "byte"),
        ElementType.I2
          => new ResValueType(source, "short"),
        ElementType.U2
          => new ResValueType(source, "ushort"),
        ElementType.I4
          => new ResValueType(source, "int"),
        ElementType.U4
          => new ResValueType(source, "uint"),
        ElementType.I8
          => new ResValueType(source, "long"),
        ElementType.U8
          => new ResValueType(source, "ulong"),
        ElementType.R4
          => new ResValueType(source, "float"),
        ElementType.R8
          => new ResValueType(source, "double"),
        _ => new ResType(source),
      };
    }
#pragma warning restore CA1822 // Mark members as static

    private static bool IsTuple(dnlib.DotNet.TypeSig source)
    {
      var name = source.ReflectionName.Remove(source.ReflectionName.IndexOf('`', StringComparison.InvariantCulture));
      if (name.Equals(nameof(Tuple), StringComparison.InvariantCulture))
        return true;

      return false;
    }

    private static bool IsGeneric(dnlib.DotNet.TypeSig source)
      => source.ReflectionName.Contains('`', StringComparison.InvariantCulture);

    private static bool FilterNamespaces(string typeXamespace)
      => !string.IsNullOrEmpty(typeXamespace)
          && !EXCLUDED_NAMESPACES.Contains(typeXamespace.Contains('.', StringComparison.InvariantCulture)
               ? typeXamespace.Remove(typeXamespace.IndexOf('.', StringComparison.InvariantCulture))
               : typeXamespace);

    private IReadOnlyDictionary<string, IReadOnlyCollection<IType>> ComposeTypes()
  => m_groups.SelectMany(x => x)
             .ToDictionary(x => x.Key, x => x.SelectMany(y => y).ToArray() as IReadOnlyCollection<IType>);

    internal IType ResolveType(dnlib.DotNet.TypeDef subject, dnlib.DotNet.TypeDef? parent = null)
    {
      var nestedParent = ResolveParent(parent);

      if (subject.IsClass)
        return new ClassDef(subject, nestedParent);
      if (subject.IsInterface)
        return new InterfaceDef(subject, nestedParent);
      if (subject.IsEnum)
        return new EnumDef(subject, nestedParent);

      throw new NotSupportedException(Resources.subjectNotSupported);
    }

    internal static IEnumerable<IType> ResolveType(dnlib.DotNet.TypeDef subject)
    {
      if (subject.IsClass)
      {
        var type = new ClassDef(subject, null);
        yield return type;
        foreach (var item in type.NestedTypes)
          yield return item;

        yield break;
      }
      if (subject.IsInterface)
      {
        var type = new InterfaceDef(subject, null);
        yield return type;
        foreach (var item in type.NestedTypes)
          yield return item;
        yield break;
      }
      if (subject.IsEnum)
      {
        yield return new EnumDef(subject, null);
        yield break;
      }

      throw new NotSupportedException(Resources.subjectNotSupported);
    }

    private static dnlib.DotNet.TypeDef? ResolveParent(object? parent)
    {
      if (parent == null)
        return null;
      if (!(parent is dnlib.DotNet.TypeDef type))
        throw new InvalidOperationException($"Argument type of {parent} is not {nameof(dnlib.DotNet.TypeDef)}.");

      return type;
    }

    #endregion
  }
}
