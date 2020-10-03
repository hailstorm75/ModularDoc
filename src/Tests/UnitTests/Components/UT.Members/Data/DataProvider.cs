using System;
using System.Collections.Generic;
using System.Linq;
using MarkDoc.Helpers;
using MarkDoc.Members;
using MarkDoc.Members.Members;
using MarkDoc.Members.Types;
using UT.Members.TypeTests;

namespace UT.Members.Data
{
  public static class DataProvider
  {
    public static IEnumerable<object> WrapItem(this object item)
      => new[] {item};

    public static IEnumerable<object[]> ComposeData<T>(this object[][] data, Func<IResolver, T> parent,
      params string[] assemblies)
      where T : class, IType
    {
      foreach (var assembly in assemblies)
      foreach (var resolver in new ResolversProvider().WhereNotNull())
      {
        resolver.Resolve(assembly);

        var result = parent(resolver);
        foreach (object[] entry in data)
          yield return result.WrapItem().Concat(entry).ToArray();
      }
    }

    public static IEnumerable<object[]> ComposeData<T>(this object[][] data, Func<IResolver, IEnumerable<T>> parent,
      params string[] assemblies)
      where T : class, IType
    {
      foreach (var assembly in assemblies)
      foreach (var resolver in new ResolversProvider().WhereNotNull())
      {
        resolver.Resolve(assembly);

        var parents = parent(resolver);
        foreach (var type in parents)
        foreach (object[] entry in data)
          yield return type.WrapItem().Concat(entry).ToArray();
      }
    }

    public static IEnumerable<T> FindMemberParents<T>(this IResolver resolver, string memberNamespace)
      where T : class, IType
      => resolver.Types.Value[memberNamespace]
        .OfType<T>();

    public static IEnumerable<T> FindMemberParents<T>(this IResolver resolver, string memberNamespace,
      ISet<string> filter)
      where T : class, IType
      => resolver.Types.Value[memberNamespace]
        .OfType<T>()
        .Where(type => filter.Contains(type.Name));

    public static T FindMemberParent<T>(this IResolver resolver, string memberNamespace, string parentName)
      where T : class, IType
      => resolver.FindMemberParents<T>(memberNamespace)
        .First(type => type.Name.Equals(parentName));

    public static T FindMember<T>(this IEnumerable<T> members, string memberName)
      where T : IMember
    {
      var member = members.First(mem => mem.Name.Equals(memberName, StringComparison.InvariantCulture));

      return member;
    }

    public static T? FindMember<T>(this IEnumerable<T> members, string memberName, bool throwIfNull)
      where T : class, IMember
    {
      var member = members.FirstOrDefault(mem => mem.Name.Equals(memberName, StringComparison.InvariantCulture));
      if (throwIfNull && member is null)
        throw new KeyNotFoundException();

      return member;
    }

    public static T? FindType<T>(this IResolver resolver, string name)
      where T : class, IType
      => resolver
        .GetTypes<T>()
        .FirstOrDefault(type => type.Name.Equals(name));
  }
}