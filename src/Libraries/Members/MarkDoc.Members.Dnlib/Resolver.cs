using dnlib.DotNet;
using MarkDoc.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Linq;
using MarkDoc.Members.Dnlib.Properties;
using System.Collections.Concurrent;
using MarkDoc.Members.Dnlib.ResolvedTypes;
using MarkDoc.Members.ResolvedTypes;

namespace MarkDoc.Members.Dnlib
{
  public class Resolver
    : IResolver
  {
    #region Fields

    private static readonly HashSet<string> EXCLUDED_NAMESPACES = new HashSet<string> { "System", "Microsoft" };
    private static readonly ConcurrentBag<IEnumerable<IGrouping<string, IReadOnlyCollection<IType>>>> m_groups = new ConcurrentBag<IEnumerable<IGrouping<string, IReadOnlyCollection<IType>>>>();
    private static readonly ConcurrentDictionary<string, IResType> m_resCache = new ConcurrentDictionary<string, IResType>();

    #endregion

    #region Properties

    public Lazy<IReadOnlyDictionary<string, IReadOnlyCollection<IType>>> Types { get; }

    #endregion

    public Resolver()
      => Types = new Lazy<IReadOnlyDictionary<string, IReadOnlyCollection<IType>>>(ComposeTypes, LazyThreadSafetyMode.ExecutionAndPublication);

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
                        .Where(x => FilterNamespaces(Linq.GroupKey(x)))
                        .GroupBy(Linq.GroupKey, x => x.Select(t => ResolveType(t)).SelectMany(Linq.XtoX).ToReadOnlyCollection());

      m_groups.Add(group);
    }

#pragma warning disable CA1822 // Mark members as static
    public IResType Resolve(object source, IReadOnlyDictionary<string, string>? generics = null)
    {
      if (source == null)
        throw new ArgumentNullException(nameof(source));

      if (!(source is TypeSig signature))
        throw new NotSupportedException(); // TODO: Message

      if (m_resCache.TryGetValue(signature.FullName, out var resolution))
        return resolution;

      var result = signature.ElementType switch
      {
        ElementType.Boolean
          => new ResValueType(this, signature, "bool"),
        ElementType.Char
          => new ResValueType(this, signature, "char"),
        ElementType.String
          => new ResValueType(this, signature, "string"),
        var x when x is ElementType.SZArray || x is ElementType.Array
          => new ResArray(this, signature, generics),
        var x when (x is ElementType.GenericInst) && IsGeneric(signature)
          => IsTuple(signature)
              ? new ResTuple(this, signature)
              : new ResGeneric(this, signature, generics) as IResType,
        var x when (x is ElementType.Var || x is ElementType.MVar)
          => new ResGenericValueType(this, signature, generics),
        ElementType.Object
          => new ResValueType(this, signature, "object"),
        ElementType.I1
          => new ResValueType(this, signature, "sbyte"),
        ElementType.U1
          => new ResValueType(this, signature, "byte"),
        ElementType.I2
          => new ResValueType(this, signature, "short"),
        ElementType.U2
          => new ResValueType(this, signature, "ushort"),
        ElementType.I4
          => new ResValueType(this, signature, "int"),
        ElementType.U4
          => new ResValueType(this, signature, "uint"),
        ElementType.I8
          => new ResValueType(this, signature, "long"),
        ElementType.U8
          => new ResValueType(this, signature, "ulong"),
        ElementType.R4
          => new ResValueType(this, signature, "float"),
        ElementType.R8
          => new ResValueType(this, signature, "double"),
        _ => new ResType(this, signature),
      };

      m_resCache.AddOrUpdate(signature.FullName, result, (x, y) => result);

      return result;
    }
#pragma warning restore CA1822 // Mark members as static

    public IType? FindReference(object source, IResType type)
    {
      if (source == null)
        throw new ArgumentNullException(nameof(source));

      if (!(source is TypeSig signature))
        throw new NotSupportedException(); // TODO: Message

      if (!Types.Value.ContainsKey(signature.Namespace))
        return null;

      IType? result = Types.Value[signature.Namespace].FirstOrDefault(x => x.RawName.Equals(type.RawName, StringComparison.InvariantCulture));
      return result;
    }

    private static bool IsTuple(TypeSig source)
    {
      var name = source.ReflectionName.Remove(source.ReflectionName.IndexOf('`', StringComparison.InvariantCulture));
      if (name.Equals(nameof(Tuple), StringComparison.InvariantCulture))
        return true;

      return false;
    }

    private static bool IsGeneric(TypeSig source)
      => source.ReflectionName.Contains('`', StringComparison.InvariantCulture);

    private static bool FilterNamespaces(string typeXamespace)
      => !string.IsNullOrEmpty(typeXamespace)
          && !EXCLUDED_NAMESPACES.Contains(typeXamespace.Contains('.', StringComparison.InvariantCulture)
               ? typeXamespace.Remove(typeXamespace.IndexOf('.', StringComparison.InvariantCulture))
               : typeXamespace);

    private IReadOnlyDictionary<string, IReadOnlyCollection<IType>> ComposeTypes()
      => m_groups.SelectMany(Linq.XtoX)
                 .ToDictionary(Linq.GroupKey, x => x.GroupValuesOfValues().ToReadOnlyCollection());

    public IType ResolveType(object subject, object? parent = null)
    {
      if (!(subject is dnlib.DotNet.TypeDef subjectSig))
        throw new NotSupportedException(); // TODO: Message

      var nestedParent = ResolveParent(parent);

      if (subjectSig.IsEnum)
        return new EnumDef(this, subjectSig, nestedParent);
      if (subjectSig.IsClass)
        return new ClassDef(this, subjectSig, nestedParent);
      if (subjectSig.IsInterface)
        return new InterfaceDef(this, subjectSig, nestedParent);

      throw new NotSupportedException(Resources.subjectNotSupported);
    }

    internal IEnumerable<IType> ResolveType(dnlib.DotNet.TypeDef subject)
    {
      static IEnumerable<IType> IterateNested(IInterface type)
      {
        if (!type.NestedTypes.Any())
          yield break;

        foreach (var nested in type.NestedTypes)
        {
          yield return nested;
          if (nested is IInterface nestedType)
            foreach (var nestedNested in IterateNested(nestedType))
              yield return nestedNested;
        }
      }

      if (subject.IsEnum)
      {
        yield return new EnumDef(this, subject, null);
        yield break;
      }
      if (subject.IsClass)
      {
        var type = new ClassDef(this, subject, null);
        yield return type;
        foreach (var item in IterateNested(type))
          yield return item;

        yield break;
      }
      if (subject.IsInterface)
      {
        var type = new InterfaceDef(this, subject, null);
        yield return type;
        foreach (var item in IterateNested(type))
          yield return item;

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

    public bool TryFindType(string fullname, out IType? result)
    {
      if (fullname == null)
        throw new ArgumentNullException(nameof(fullname));

      result = null;

      //var genericIndex = fullname.LastIndexOf('`');
      //if (genericIndex != -1)
      //  fullname = fullname.Remove(genericIndex);

      var index = fullname.LastIndexOf('.');
      if (index == -1)
        return false;

      var namespaceCut = fullname.Remove(index);
      var types = Types.Value[namespaceCut];
      result = types.FirstOrDefault(x => x.RawName.Equals(fullname, StringComparison.InvariantCulture));

      return result != null;
    }

    #endregion
  }
}
