using System;
using System.Collections.Generic;
using System.Linq;
using MarkDoc.Helpers;
using MarkDoc.Members;
using MarkDoc.Members.Enums;
using MarkDoc.Members.Types;
using UT.Members.Data;
using Xunit;

namespace UT.Members
{
  public class InterfaceTests
  {
    #region Data providers

    private static IEnumerable<object[]> GetInterfaceNames()
    {
      var data = new[]
      {
        Constants.PUBLIC_INTERFACE,
        Constants.INTERNAL_INTERFACE,
        Constants.PUBLIC_NESTED_INTERFACE,
        Constants.PROTECTED_NESTED_INTERFACE,
        Constants.INTERNAL_NESTED_INTERFACE,
        Constants.INTERNAL_GENERIC_INTERFACE,
        Constants.INTERNAL_NESTED_GENERIC_INTERFACE
      };

      foreach (var name in data)
        foreach (var resolver in new ResolversProvider())
          yield return new[] { resolver.First(), name };
    }

    private static IEnumerable<object[]> GetInterfaceAccessorsData()
    {
      var data = new[]
      {
        (Constants.PUBLIC_INTERFACE, AccessorType.Public),
        (Constants.INTERNAL_INTERFACE, AccessorType.Internal),
        (Constants.PUBLIC_NESTED_INTERFACE, AccessorType.Public),
        (Constants.PROTECTED_NESTED_INTERFACE, AccessorType.Protected),
        (Constants.INTERNAL_NESTED_INTERFACE, AccessorType.Internal),
      };

      foreach (var (name, accessor) in data)
        foreach (var resolver in new ResolversProvider())
          yield return new[] { resolver.First(), name, accessor };
    }

    private static IEnumerable<object[]> GetInterfaceNamespaceData()
    {
      const string interfaceNameSpace = "TestLibrary.Interfaces";
      var data = new[]
      {
        (Constants.PUBLIC_INTERFACE, interfaceNameSpace),
        (Constants.INTERNAL_INTERFACE, interfaceNameSpace),
        (Constants.PUBLIC_NESTED_INTERFACE, $"{interfaceNameSpace}.InterfaceParent"),
        (Constants.PROTECTED_NESTED_INTERFACE, $"{interfaceNameSpace}.InterfaceParent"),
        (Constants.INTERNAL_NESTED_INTERFACE, $"{interfaceNameSpace}.InterfaceParent"),
      };

      foreach (var (name, space) in data)
        foreach (var resolver in new ResolversProvider())
          yield return new[] { resolver.First(), name, space };
    }

    private static IEnumerable<object[]> GetInterfaceGenericData()
    {
      var generics = new Dictionary<string, (Variance, IReadOnlyCollection<string>)>
      {
        { "T1", (Variance.NonVariant, new string[]{}) },
        { "T2", (Variance.Contravariant, new []{ Constants.PUBLIC_INTERFACE }) },
        { "T3", (Variance.Covariant, new []{ nameof(IDisposable) }) },
      };

      var data = new[]
      {
        Constants.INTERNAL_GENERIC_INTERFACE,
        Constants.INTERNAL_NESTED_GENERIC_INTERFACE
      };

      foreach (var name in data)
        foreach (var resolver in new ResolversProvider())
          yield return new[] { resolver.First(), name, generics };
    }

    #endregion

    private static IInterface? GetInterface(IResolver resolver, string name)
    {
      resolver.Resolve(Constants.TEST_ASSEMBLY);

      return resolver
        .GetTypes<IInterface>()
        .FirstOrDefault(type => type.Name.Equals(name));
    }

    [Theory]
    [Trait("Category",nameof(IInterface))]
    [MemberData(nameof(GetInterfaceNames))]
    public void ValidateInterfaceNames(IResolver resolver, string name)
    {
      var query = GetInterface(resolver, name);

      Assert.NotNull(query);
    }

    [Theory]
    [Trait("Category",nameof(IInterface))]
    [MemberData(nameof(GetInterfaceAccessorsData))]
    public void ValidateInterfaceAccessors(IResolver resolver, string name, AccessorType accessor)
    {
      var query = GetInterface(resolver, name);

      Assert.True(query?.Accessor == accessor, $"{resolver.GetType().FullName}: The '{name}' accessor type is invalid. Expected '{accessor}' != Actual '{query?.Accessor}'");
    }

    [Theory]
    [Trait("Category",nameof(IInterface))]
    [MemberData(nameof(GetInterfaceNamespaceData))]
    public void ValidateInterfaceRawName(IResolver resolver, string name, string expectedNamespace)
    {
      var query = GetInterface(resolver, name);

      Assert.True(query?.RawName.Equals($"{expectedNamespace}.{name}"), $"{resolver.GetType().FullName}: The '{name}' raw name is invalid. Expected '{expectedNamespace}.{name}' != Actual '{query?.RawName}'.");
    }

    [Theory]
    [Trait("Category",nameof(IInterface))]
    [MemberData(nameof(GetInterfaceGenericData))]
    public void ValidateInterfaceGenericVariances(IResolver resolver, string name, Dictionary<string, (Variance variance, IReadOnlyCollection<string>)> generics)
    {
      var query = GetInterface(resolver, name);

      var interfaceGenerics = query?.Generics
        .Select(item => (item.Key, item.Value.variance))
        .OrderBy(key => key.Key);

      var expectedGenerics = generics
        .Select(item => (item.Key, item.Value.variance))
        .OrderBy(key => key.Key);

      Assert.Equal(expectedGenerics, interfaceGenerics);
    }

    [Theory]
    [Trait("Category",nameof(IInterface))]
    [MemberData(nameof(GetInterfaceGenericData))]
    public void ValidateInterfaceGenericConstraints(IResolver resolver, string name, Dictionary<string, (Variance, IReadOnlyCollection<string> constraints)> generics)
    {
      var query = GetInterface(resolver, name);

      var actualGenerics = query?.Generics
        .Select(item => (item.Key, string.Join(";", item.Value.constraints.Select(constraint => constraint.DisplayName).OrderBy(Linq.XtoX))))
        .OrderBy(key => key.Key);

      var expectedGenerics = generics
        .Select(item => (item.Key, string.Join(";", item.Value.constraints.OrderBy(Linq.XtoX))))
        .OrderBy(key => key.Key);

      Assert.Equal(expectedGenerics, actualGenerics);
    }
  }
}