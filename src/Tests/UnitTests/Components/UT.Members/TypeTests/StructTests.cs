using System;
using System.Collections.Generic;
using System.Linq;
using MarkDoc.Helpers;
using MarkDoc.Members;
using MarkDoc.Members.Enums;
using MarkDoc.Members.Members;
using MarkDoc.Members.Types;
using UT.Members.Data;
using Xunit;

namespace UT.Members.TypeTests
{
  public class StructTests
  {
    #region Data providers

    public static IEnumerable<object?[]> GetStructNames()
    {
      var data = new[]
      {
        Constants.PUBLIC_STRUCT,
        Constants.INTERNAL_STRUCT,
        Constants.PUBLIC_NESTED_STRUCT,
        Constants.PROTECTED_NESTED_STRUCT,
        Constants.PROTECTED_INTERNAL_NESTED_STRUCT,
        Constants.INTERNAL_NESTED_STRUCT,
      };

      return data.ComposeData();
    }

    public static IEnumerable<object[]> GetStructAccessorsData()
    {
      var data = new[]
      {
        new object[] {Constants.PUBLIC_STRUCT, AccessorType.Public},
        new object[] {Constants.INTERNAL_STRUCT, AccessorType.Internal},
        new object[] {Constants.PUBLIC_NESTED_STRUCT, AccessorType.Public},
        new object[] {Constants.PROTECTED_NESTED_STRUCT, AccessorType.Protected},
        new object[] {Constants.PROTECTED_INTERNAL_NESTED_STRUCT, AccessorType.ProtectedInternal},
        new object[] {Constants.INTERNAL_NESTED_STRUCT, AccessorType.Internal},
      };

      return data.ComposeData();
    }

    public static IEnumerable<object[]> GetStructNamespaceData()
    {
      var data = new[]
      {
        new object[] {Constants.PUBLIC_STRUCT, Constants.STRUCT_NAMESPACE},
        new object[] {Constants.INTERNAL_STRUCT, Constants.STRUCT_NAMESPACE},
        new object[] {Constants.PUBLIC_NESTED_STRUCT, $"{Constants.STRUCT_NAMESPACE}.StructParent"},
        new object[] {Constants.PROTECTED_NESTED_STRUCT, $"{Constants.STRUCT_NAMESPACE}.StructParent"},
        new object[] {Constants.PROTECTED_INTERNAL_NESTED_STRUCT, $"{Constants.STRUCT_NAMESPACE}.StructParent"},
        new object[] {Constants.INTERNAL_NESTED_STRUCT, $"{Constants.STRUCT_NAMESPACE}.StructParent"},
      };

      return data.ComposeData();
    }

    public static IEnumerable<object[]> GetStructIsReadonlyData()
    {
      var data = new[]
      {
        new object[] {Constants.PUBLIC_STRUCT, false},
        new object[] {Constants.PUBLIC_STRUCT_READONLY, true},
      };

      return data.ComposeData();
    }

    public static IEnumerable<object[]> GetStructWithMembersData()
      => new ResolversProvider().Select(resolver => new object[] { resolver, Constants.PUBLIC_STRUCT });

    private static IEnumerable<object[]> GetStructMemberData(string member)
      => new ResolversProvider().Select(resolver => new object[] { resolver, Constants.PUBLIC_STRUCT, member });

    public static IEnumerable<object[]> GetStructPropertiesData()
      => GetStructMemberData("Property");

    public static IEnumerable<object[]> GetStructMethodsData()
      => GetStructMemberData("Method");

    public static IEnumerable<object[]> GetStructEventsData()
      => GetStructMemberData("Event");

    public static IEnumerable<object[]> GetStructDelegatesData()
      => GetStructMemberData("Delegate");

    public static IEnumerable<object[]> GetStructGenericData()
    {
      var generics = new Dictionary<string, (Variance, IReadOnlyCollection<string>)>
      {
        { "T1", (Variance.NonVariant, new string[]{}) },
        { "T2", (Variance.NonVariant, new []{ nameof(IDisposable) }) },
      };

      var data = new[]
      {
        new object[] { Constants.PUBLIC_GENERIC_STRUCT, generics },
        new object[] { Constants.PUBLIC_NESTED_GENERIC_STRUCT, generics }
      };

      return data.ComposeData();
    }

    private static IEnumerable<object[]> GetStructInheritedMembersData(string member)
    {
      var data = new[]
      {
        new object[] {Constants.PUBLIC_INHERITING_STRUCT, member, Constants.PUBLIC_INHERITED_INTERFACE},
        new object[] {Constants.PUBLIC_INHERITING_STRUCT_EMPTY, member, Constants.PUBLIC_INHERITED_INTERFACE},
        new object[] {Constants.PUBLIC_INHERITING_COMPLEX_STRUCT, member, Constants.PUBLIC_INHERITED_INTERFACE},
        new object[] {Constants.PUBLIC_INHERITING_COMPLEX_STRUCT_EMPTY, member, Constants.PUBLIC_INHERITED_INTERFACE},
        new object[] {Constants.PUBLIC_INHERITING_COMPLEX_STRUCT, $"Other{member}", Constants.PUBLIC_INHERITING_AND_INHERITED_INTERFACE},
        new object[] {Constants.PUBLIC_INHERITING_COMPLEX_STRUCT_EMPTY, $"Other{member}", Constants.PUBLIC_INHERITING_AND_INHERITED_INTERFACE},
      };

      return data.ComposeData();
    }

    public static IEnumerable<object[]> GetStructInheritedEventsData()
      => GetStructInheritedMembersData("Event");

    public static IEnumerable<object[]> GetStructInheritedDelegatesData()
      => GetStructInheritedMembersData("Delegate");

    public static IEnumerable<object[]> GetStructInheritedPropertiesData()
      => GetStructInheritedMembersData("Property");

    public static IEnumerable<object[]> GetStructInheritedMethodsData()
      => GetStructInheritedMembersData("Method");

    public static IEnumerable<object[]> GetStructConstructorData()
    {
      var data = new[]
      {
        new object[] { Constants.PUBLIC_STRUCT_EMPTY_CTOR, 0 },
        new object[] { Constants.PUBLIC_STRUCT_PARAM_CTOR, 1 },
      };

      return data.ComposeData();
    }

    private static IStruct? GetStruct(IResolver resolver, string name)
    {
      resolver.Resolve(Constants.TEST_ASSEMBLY);

      return resolver.FindType<IStruct>(name);
    }

    #endregion

    [Theory]
    [Trait("Category", nameof(IStruct))]
    [MemberData(nameof(GetStructNames))]
    public void ValidateStructNames(IResolver resolver, string name)
    {
      var query = GetStruct(resolver, name);

      Assert.NotNull(query);
    }

    [Theory]
    [Trait("Category", nameof(IStruct))]
    [MemberData(nameof(GetStructIsReadonlyData))]
    public void ValidateStructIsReadonly(IResolver resolver, string name, bool isReadonly)
    {
      var query = GetStruct(resolver, name);

      Assert.Equal(isReadonly, query?.IsReadOnly);
    }

    [Theory]
    [Trait("Category", nameof(IStruct))]
    [MemberData(nameof(GetStructNamespaceData))]
    public void ValidateStructRawName(IResolver resolver, string name, string expectedNamespace)
    {
      var query = GetStruct(resolver, name);

      Assert.True(query?.RawName.Equals($"{expectedNamespace}.{name}"), $"{resolver.GetType().FullName}: The '{name}' raw name is invalid. Expected '{expectedNamespace}.{name}' != Actual '{query?.RawName}'.");
    }

    [Theory]
    [Trait("Category", nameof(IStruct))]
    [MemberData(nameof(GetStructAccessorsData))]
    public void ValidateStructAccessors(IResolver resolver, string name, AccessorType accessor)
    {
      var query = GetStruct(resolver, name);

      Assert.True(query?.Accessor == accessor, $"{resolver.GetType().FullName}: The '{name}' accessor type is invalid. Expected '{accessor}' != Actual '{query?.Accessor}'");
    }

    [Theory]
    [Trait("Category", nameof(IEnum))]
    [MemberData(nameof(GetStructNamespaceData))]
    public void ValidateStructNamespace(IResolver resolver, string name, string expectedNamespace)
    {
      var query = GetStruct(resolver, name);

      Assert.True(query?.TypeNamespace.Equals(expectedNamespace), $"{resolver.GetType().FullName}: The '{name}' namespace is invalid. Expected '{expectedNamespace}' != Actual '{query?.TypeNamespace}'.");
    }

    [Theory]
    [Trait("Category", nameof(IProperty))]
    [MemberData(nameof(GetStructPropertiesData))]
    public void ValidateStructProperties(IResolver resolver, string name, string member)
    {
      var query = GetStruct(resolver, name);

      var hasProperty = query?.Properties.Any(property => property.Name.Equals(member)) ?? false;

      Assert.True(hasProperty, $"{resolver.GetType().FullName}: The '{name}' struct is missing the '{member}'.");
    }

    [Theory]
    [Trait("Category", nameof(IMethod))]
    [MemberData(nameof(GetStructMethodsData))]
    public void ValidateStructMethods(IResolver resolver, string name, string member)
    {
      var query = GetStruct(resolver, name);

      var hasMethod = query?.Methods.Any(method => method.Name.Equals(member)) ?? false;

      Assert.True(hasMethod, $"{resolver.GetType().FullName}: The '{name}' struct is missing the '{member}'.");
    }

    [Theory]
    [Trait("Category", nameof(IEvent))]
    [MemberData(nameof(GetStructEventsData))]
    public void ValidateStructEvents(IResolver resolver, string name, string member)
    {
      var query = GetStruct(resolver, name);

      var hasEvent = query?.Events.Any(@event => @event.Name.Equals(member)) ?? false;

      Assert.True(hasEvent, $"{resolver.GetType().FullName}: The '{name}' struct is missing the '{member}'.");
    }

    [Theory]
    [Trait("Category", nameof(IDelegate))]
    [MemberData(nameof(GetStructDelegatesData))]
    public void ValidateStructDelegates(IResolver resolver, string name, string member)
    {
      var query = GetStruct(resolver, name);

      var hasDelegate = query?.Delegates.Any(@delegate => @delegate.Name.Equals(member)) ?? false;

      Assert.True(hasDelegate, $"{resolver.GetType().FullName}: The '{name}' struct is missing the '{member}'.");
    }

    [Theory]
    [Trait("Category", nameof(IProperty))]
    [MemberData(nameof(GetStructWithMembersData))]
    public void ValidateStructPropertiesCount(IResolver resolver, string name)
    {
      var query = GetStruct(resolver, name);

      var propertiesCount = query?.Properties.Count ?? 0;

      Assert.True(propertiesCount == 1, $"{resolver.GetType().FullName}: The '{name}' struct has an unexpected number of members than expected.");
    }

    [Theory]
    [Trait("Category", nameof(IMethod))]
    [MemberData(nameof(GetStructWithMembersData))]
    public void ValidateStructMethodsCount(IResolver resolver, string name)
    {
      var query = GetStruct(resolver, name);

      var methodsCount = query?.Methods.Count ?? 0;

      Assert.True(methodsCount == 1, $"{resolver.GetType().FullName}: The '{name}' struct has an unexpected number of members than expected.");
    }

    [Theory]
    [Trait("Category", nameof(IEvent))]
    [MemberData(nameof(GetStructWithMembersData))]
    public void ValidateStructEventsCount(IResolver resolver, string name)
    {
      var query = GetStruct(resolver, name);

      var eventsCount = query?.Events.Count ?? 0;

      Assert.True(eventsCount == 1, $"{resolver.GetType().FullName}: The '{name}' struct has an unexpected number of members than expected.");
    }

    [Theory]
    [Trait("Category", nameof(IDelegate))]
    [MemberData(nameof(GetStructWithMembersData))]
    public void ValidateStructDelegatesCount(IResolver resolver, string name)
    {
      var query = GetStruct(resolver, name);

      var delegatesCount = query?.Delegates.Count ?? 0;

      Assert.True(delegatesCount == 1, $"{resolver.GetType().FullName}: The '{name}' struct has an unexpected number of members than expected.");
    }

    [Theory]
    [Trait("Category", nameof(IEnum))]
    [MemberData(nameof(GetStructWithMembersData))]
    public void ValidateStructNestedTypeEnum(IResolver resolver, string name)
    {
      var query = GetStruct(resolver, name);

      var enumType = query?.NestedTypes.OfType<IEnum>().FirstOrDefault(nested => nested.Name.Equals("MyEnum"));

      Assert.False(enumType is null, $"{resolver.GetType().FullName}: The '{name}' struct is missing the expected nested enum.");
    }

    [Theory]
    [Trait("Category", nameof(IEnum))]
    [MemberData(nameof(GetStructWithMembersData))]
    public void ValidateStructNestedTypeEnumCount(IResolver resolver, string name)
    {
      var query = GetStruct(resolver, name);

      var enumCount = query?.NestedTypes.OfType<IEnum>().Count() ?? 0;

      Assert.True(enumCount == 1, $"{resolver.GetType().FullName}: The '{name}' struct has an unexpected number of nested enums.");
    }

    [Theory]
    [Trait("Category", nameof(IStruct))]
    [MemberData(nameof(GetStructWithMembersData))]
    public void ValidateStructNestedTypeStruct(IResolver resolver, string name)
    {
      var query = GetStruct(resolver, name);

      var structType = query?.NestedTypes.OfType<IStruct>().FirstOrDefault(nested => nested.Name.Equals("MyStruct"));

      Assert.False(structType is null, $"{resolver.GetType().FullName}: The '{name}' struct is missing the expected nested struct.");
    }

    [Theory]
    [Trait("Category", nameof(IStruct))]
    [MemberData(nameof(GetStructWithMembersData))]
    public void ValidateStructNestedTypeStructCount(IResolver resolver, string name)
    {
      var query = GetStruct(resolver, name);

      var structCount = query?.NestedTypes.OfType<IStruct>().Count() ?? 0;

      Assert.True(structCount == 1, $"{resolver.GetType().FullName}: The '{name}' struct has an unexpected number of nested structs.");
    }

    [Theory]
    [Trait("Category", nameof(IClass))]
    [MemberData(nameof(GetStructWithMembersData))]
    public void ValidateStructNestedTypeClass(IResolver resolver, string name)
    {
      var query = GetStruct(resolver, name);

      var classType = query?.NestedTypes.OfType<IClass>().FirstOrDefault(nested => nested.Name.Equals("MyClass"));

      Assert.False(classType is null, $"{resolver.GetType().FullName}: The '{name}' struct is missing the expected nested class.");
    }

    [Theory]
    [Trait("Category", nameof(IClass))]
    [MemberData(nameof(GetStructWithMembersData))]
    public void ValidateStructNestedTypeClassCount(IResolver resolver, string name)
    {
      var query = GetStruct(resolver, name);

      var classCount = query?.NestedTypes.OfType<IClass>().Count() ?? 0;

      Assert.True(classCount == 1, $"{resolver.GetType().FullName}: The '{name}' struct has an unexpected number of nested classes.");
    }

    [Theory]
    [Trait("Category", nameof(IStruct))]
    [MemberData(nameof(GetStructGenericData))]
    public void ValidateStructGenericVariances(IResolver resolver, string name, Dictionary<string, (Variance variance, IReadOnlyCollection<string>)> generics)
    {
      var query = GetStruct(resolver, name);

      var interfaceGenerics = query?.Generics
        .Select(item => (item.Key, item.Value.variance))
        .OrderBy(key => key.Key);

      var expectedGenerics = generics
        .Select(item => (item.Key, item.Value.variance))
        .OrderBy(key => key.Key);

      Assert.Equal(expectedGenerics, interfaceGenerics);
    }

    [Theory]
    [Trait("Category", nameof(IStruct))]
    [MemberData(nameof(GetStructGenericData))]
    public void ValidateStructGenericConstraints(IResolver resolver, string name, Dictionary<string, (Variance, IReadOnlyCollection<string> constraints)> generics)
    {
      var query = GetStruct(resolver, name);

      var actualGenerics = query?.Generics
        .Select(item => (item.Key, string.Join(";", item.Value.constraints.Select(constraint => constraint.DisplayName).OrderBy(Linq.XtoX))))
        .OrderBy(key => key.Key);

      var expectedGenerics = generics
        .Select(item => (item.Key, string.Join(";", item.Value.constraints.OrderBy(Linq.XtoX))))
        .OrderBy(key => key.Key);

      Assert.Equal(expectedGenerics, actualGenerics);
    }

    [Theory]
    [Trait("Category", nameof(IStruct))]
    [Trait("Category", nameof(IEvent))]
    [MemberData(nameof(GetStructInheritedEventsData))]
    public void ValidateInheritedEventMembers(IResolver resolver, string name, string memberName, string sourceTypeName)
    {
      var query = GetStruct(resolver, name);

      var inheritedMember = query?.InheritedTypes.Value.FirstOrDefault(member => member.Key.Name.Equals(memberName)) ?? default;

      Assert.Equal(sourceTypeName, inheritedMember.Value.Name);
    }

    [Theory]
    [Trait("Category", nameof(IStruct))]
    [Trait("Category", nameof(IDelegate))]
    [MemberData(nameof(GetStructInheritedDelegatesData))]
    public void ValidateInheritedDelegateMembers(IResolver resolver, string name, string memberName, string sourceTypeName)
    {
      var query = GetStruct(resolver, name);

      var inheritedMember = query?.InheritedTypes.Value.FirstOrDefault(member => member.Key.Name.Equals(memberName)) ?? default;

      Assert.Equal(sourceTypeName, inheritedMember.Value.Name);
    }

    [Theory]
    [Trait("Category", nameof(IStruct))]
    [Trait("Category", nameof(IProperty))]
    [MemberData(nameof(GetStructInheritedPropertiesData))]
    public void ValidateInheritedPropertyMembers(IResolver resolver, string name, string memberName, string sourceTypeName)
    {
      var query = GetStruct(resolver, name);

      var inheritedMember = query?.InheritedTypes.Value.FirstOrDefault(member => member.Key.Name.Equals(memberName)) ?? default;

      Assert.Equal(sourceTypeName, inheritedMember.Value.Name);
    }

    [Theory]
    [Trait("Category", nameof(IStruct))]
    [Trait("Category", nameof(IMethod))]
    [MemberData(nameof(GetStructInheritedMethodsData))]
    public void ValidateInheritedMethodMembers(IResolver resolver, string name, string memberName, string sourceTypeName)
    {
      var query = GetStruct(resolver, name);

      var inheritedMember = query?.InheritedTypes.Value.FirstOrDefault(member => member.Key.Name.Equals(memberName)) ?? default;

      Assert.Equal(sourceTypeName, inheritedMember.Value.Name);
    }

    [Theory]
    [Trait("Category", nameof(IStruct))]
    [MemberData(nameof(GetStructConstructorData))]
    public void ValidateClassConstructorsCount(IResolver resolver, string name, int count)
    {
      var query = GetStruct(resolver, name);

      var constructors = query?.Constructors;

      Assert.True(constructors?.Count == count);
    }
  }
}
