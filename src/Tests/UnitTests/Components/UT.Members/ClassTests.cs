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

namespace UT.Members
{
  public class ClassTests
  {
    #region Data providers

    private static IEnumerable<object?[]> GetClassNamesData()
    {
      var data = new[]
      {
        Constants.PUBLIC_CLASS,
        Constants.INTERNAL_CLASS,
        Constants.PUBLIC_NESTED_CLASS,
        Constants.PROTECTED_NESTED_CLASS,
        Constants.INTERNAL_NESTED_CLASS,
        Constants.PUBLIC_GENERIC_CLASS,
        Constants.PUBLIC_NESTED_GENERIC_CLASS
      };

      return data.ComposeData();
    }

    private static IEnumerable<object[]> GetClassNamespacesData()
    {
      const string classNameSpace = "TestLibrary.Classes";

      var data = new[]
      {
        new object[] { Constants.PUBLIC_CLASS, classNameSpace },
        new object[] { Constants.INTERNAL_CLASS, classNameSpace },
        new object[] { Constants.PUBLIC_NESTED_CLASS, $"{classNameSpace}.ClassParent" },
        new object[] { Constants.PROTECTED_NESTED_CLASS, $"{classNameSpace}.ClassParent"},
        new object[] { Constants.INTERNAL_NESTED_CLASS, $"{classNameSpace}.ClassParent"}
      };

      return data.ComposeData();
    }

    private static IEnumerable<object[]> GetClassAccessorsData()
    {
      var data = new[]
      {
        new object[] {Constants.PUBLIC_CLASS, AccessorType.Public},
        new object[] {Constants.INTERNAL_CLASS, AccessorType.Internal},
        new object[] {Constants.PUBLIC_NESTED_CLASS, AccessorType.Public},
        new object[] {Constants.PROTECTED_NESTED_CLASS, AccessorType.Protected},
        new object[] {Constants.INTERNAL_NESTED_CLASS, AccessorType.Internal},
      };

      return data.ComposeData();
    }

    private static IEnumerable<object[]> GetClassAbstractData()
    {
      var data = new[]
      {
        new object[] { Constants.PUBLIC_CLASS, false },
        new object[] { Constants.PUBLIC_CLASS_STATIC, false },
        new object[] { Constants.PUBLIC_CLASS_SEALED, false },
        new object[] { Constants.PUBLIC_CLASS_ABSTRACT, true }
      };

      return data.ComposeData();
    }

    private static IEnumerable<object[]> GetClassSealedData()
    {
      var data = new[]
      {
        new object[] { Constants.PUBLIC_CLASS, false },
        new object[] { Constants.PUBLIC_CLASS_STATIC, false },
        new object[] { Constants.PUBLIC_CLASS_ABSTRACT, false },
        new object[] { Constants.PUBLIC_CLASS_SEALED, true }
      };

      return data.ComposeData();
    }

    private static IEnumerable<object[]> GetClassStaticData()
    {
      var data = new[]
      {
        new object[] { Constants.PUBLIC_CLASS, false },
        new object[] { Constants.PUBLIC_CLASS_ABSTRACT, false },
        new object[] { Constants.PUBLIC_CLASS_SEALED, false },
        new object[] { Constants.PUBLIC_CLASS_STATIC, true }
      };

      return data.ComposeData();
    }

    private static IEnumerable<object[]> GetClassBaseClassData()
    {
      var data = new[]
      {
        new object[] { Constants.PUBLIC_CLASS, false },
        new object[] { Constants.PUBLIC_INHERITING_COMPLEX_CLASS_BASE, true },
        new object[] { Constants.PUBLIC_INHERITING_COMPLEX_CLASS_BASE_EMPTY, true },
        new object[] { Constants.PUBLIC_INHERITING_COMPLEX_CLASS, false },
        new object[] { Constants.PUBLIC_INHERITING_COMPLEX_CLASS_EMPTY, false },
        new object[] { Constants.PUBLIC_INHERITING_CLASS_BASE, true },
        new object[] { Constants.PUBLIC_INHERITING_CLASS_BASE_EMPTY, true },
        new object[] { Constants.PUBLIC_INHERITING_CLASS_EMPTY, false },
        new object[] { Constants.PUBLIC_INHERITING_CLASS, false },
      };

      return data.ComposeData();
    }

    private static IEnumerable<object[]> GetClassGenericData()
    {
      var generics = new Dictionary<string, (Variance, IReadOnlyCollection<string>)>
      {
        { "T1", (Variance.NonVariant, new string[]{}) },
        { "T2", (Variance.NonVariant, new []{ nameof(IDisposable) }) },
      };

      var data = new[]
      {
        new object[] { Constants.PUBLIC_GENERIC_CLASS, generics },
        new object[] { Constants.PUBLIC_NESTED_GENERIC_CLASS, generics },
      };

      return data.ComposeData();
    }

    private static IEnumerable<object[]> GetClassInheritedMembersData(string member)
    {
      var data = new[]
      {
        new object[] {Constants.PUBLIC_INHERITING_CLASS, member, Constants.PUBLIC_INHERITED_INTERFACE},
        new object[] {Constants.PUBLIC_INHERITING_CLASS_EMPTY, member, Constants.PUBLIC_INHERITED_INTERFACE},
        new object[] {Constants.PUBLIC_INHERITING_CLASS_BASE, $"Base{member}", Constants.PUBLIC_CLASS_ABSTRACT},
        new object[] {Constants.PUBLIC_INHERITING_CLASS_BASE_EMPTY, $"Base{member}", Constants.PUBLIC_CLASS_ABSTRACT},
        new object[] {Constants.PUBLIC_INHERITING_COMPLEX_CLASS, member, Constants.PUBLIC_INHERITED_INTERFACE},
        new object[] {Constants.PUBLIC_INHERITING_COMPLEX_CLASS_EMPTY, member, Constants.PUBLIC_INHERITED_INTERFACE},
        new object[] {Constants.PUBLIC_INHERITING_COMPLEX_CLASS, $"Other{member}", Constants.PUBLIC_INHERITING_AND_INHERITED_INTERFACE},
        new object[] {Constants.PUBLIC_INHERITING_COMPLEX_CLASS_EMPTY, $"Other{member}", Constants.PUBLIC_INHERITING_AND_INHERITED_INTERFACE},
        new object[] {Constants.PUBLIC_INHERITING_COMPLEX_CLASS_BASE, member, Constants.PUBLIC_INHERITED_INTERFACE},
        new object[] {Constants.PUBLIC_INHERITING_COMPLEX_CLASS_BASE_EMPTY, member, Constants.PUBLIC_INHERITED_INTERFACE},
        new object[] {Constants.PUBLIC_INHERITING_COMPLEX_CLASS_BASE, $"Base{member}", Constants.PUBLIC_CLASS_ABSTRACT},
        new object[] {Constants.PUBLIC_INHERITING_COMPLEX_CLASS_BASE_EMPTY, $"Base{member}", Constants.PUBLIC_CLASS_ABSTRACT},
        new object[] {Constants.PUBLIC_INHERITING_COMPLEX_CLASS_BASE, $"Abstract{member}", Constants.PUBLIC_CLASS_ABSTRACT},
        new object[] {Constants.PUBLIC_INHERITING_COMPLEX_CLASS_BASE_EMPTY, $"Abstract{member}", Constants.PUBLIC_CLASS_ABSTRACT},
        new object[] {Constants.PUBLIC_INHERITING_COMPLEX_CLASS_BASE, $"Virtual{member}", Constants.PUBLIC_CLASS_ABSTRACT},
        new object[] {Constants.PUBLIC_INHERITING_COMPLEX_CLASS_BASE_EMPTY, $"Virtual{member}", Constants.PUBLIC_CLASS_ABSTRACT},
      };

      return data.ComposeData();
    }

    private static IEnumerable<object[]> GetClassInheritedEventsData()
      => GetClassInheritedMembersData("Event");

    private static IEnumerable<object[]> GetClassInheritedDelegatesData()
      => GetClassInheritedMembersData("Delegate").Where(x => !((string)x[2]).StartsWith("Abstract", StringComparison.InvariantCultureIgnoreCase) && !((string)x[2]).StartsWith("Virtual", StringComparison.InvariantCultureIgnoreCase));

    private static IEnumerable<object[]> GetClassInheritedPropertiesData()
      => GetClassInheritedMembersData("Property");

    private static IEnumerable<object[]> GetClassInheritedMethodsData()
      => GetClassInheritedMembersData("Method");

    private static IEnumerable<object[]> GetClassWithMembersData()
      => new ResolversProvider().Select(resolver => new[] { resolver.First(), Constants.PUBLIC_CLASS });

    private static IClass? GetClass(IResolver resolver, string name)
    {
      resolver.Resolve(Constants.TEST_ASSEMBLY);

      return resolver
        .GetTypes<IClass>()
        .FirstOrDefault(type => type.Name.Equals(name));
    }

    #endregion

    [Theory]
    [Trait("Category", nameof(IClass))]
    [MemberData(nameof(GetClassNamesData))]
    public void ValidateClassNames(IResolver resolver, string name)
    {
      var query = GetClass(resolver, name);

      Assert.NotNull(query);
    }

    [Theory]
    [Trait("Category", nameof(IClass))]
    [MemberData(nameof(GetClassNamespacesData))]
    public void ValidateClassRawNames(IResolver resolver, string name, string expected)
    {
      var query = GetClass(resolver, name);

      Assert.True(query?.RawName.Equals($"{expected}.{name}"), $"{resolver.GetType().FullName}: The '{name}' raw name is invalid. Expected '{expected}.{name}' != Actual '{query?.RawName}'.");
    }

    [Theory]
    [Trait("Category", nameof(IClass))]
    [MemberData(nameof(GetClassAccessorsData))]
    public void ValidateClassAccessors(IResolver resolver, string name, AccessorType accessor)
    {
      var query = GetClass(resolver, name);

      Assert.True(query?.Accessor == accessor, $"{resolver.GetType().FullName}: The '{name}' accessor type is invalid. Expected '{accessor}' != Actual '{query?.Accessor}'");
    }

    [Theory]
    [Trait("Category", nameof(IClass))]
    [MemberData(nameof(GetClassAbstractData))]
    public void ValidateClassAbstract(IResolver resolver, string name, bool expected)
    {
      var query = GetClass(resolver, name);

      Assert.Equal(expected, query?.IsAbstract);
    }

    [Theory]
    [Trait("Category", nameof(IClass))]
    [MemberData(nameof(GetClassSealedData))]
    public void ValidateClassSealed(IResolver resolver, string name, bool expected)
    {
      var query = GetClass(resolver, name);

      Assert.Equal(expected, query?.IsSealed);
    }

    [Theory]
    [Trait("Category", nameof(IClass))]
    [MemberData(nameof(GetClassStaticData))]
    public void ValidateClassStatic(IResolver resolver, string name, bool expected)
    {
      var query = GetClass(resolver, name);

      Assert.Equal(expected, query?.IsStatic);
    }

    [Theory]
    [Trait("Category", nameof(IClass))]
    [MemberData(nameof(GetClassNamespacesData))]
    public void ValidateClassNamespace(IResolver resolver, string name, string expected)
    {
      var query = GetClass(resolver, name);

      Assert.True(query?.TypeNamespace.Equals(expected), $"{resolver.GetType().FullName}: The '{name}' namespace is invalid. Expected '{expected}' != Actual '{query?.TypeNamespace}'.");
    }

    [Theory]
    [Trait("Category", nameof(IClass))]
    [MemberData(nameof(GetClassBaseClassData))]
    public void ValidateClassHasBase(IResolver resolver, string name, bool expected)
    {
      var query = GetClass(resolver, name);

      Assert.Equal(expected, query?.BaseClass != null);
    }

    [Theory]
    [Trait("Category", nameof(IClass))]
    [MemberData(nameof(GetClassGenericData))]
    public void ValidateClassGenericVariances(IResolver resolver, string name, Dictionary<string, (Variance variance, IReadOnlyCollection<string>)> generics)
    {
      var query = GetClass(resolver, name);

      var interfaceGenerics = query?.Generics
        .Select(item => (item.Key, item.Value.variance))
        .OrderBy(key => key.Key);

      var expectedGenerics = generics
        .Select(item => (item.Key, item.Value.variance))
        .OrderBy(key => key.Key);

      Assert.Equal(expectedGenerics, interfaceGenerics);
    }

    [Theory]
    [Trait("Category", nameof(IClass))]
    [MemberData(nameof(GetClassGenericData))]
    public void ValidateClassGenericConstraints(IResolver resolver, string name, Dictionary<string, (Variance, IReadOnlyCollection<string> constraints)> generics)
    {
      var query = GetClass(resolver, name);

      var actualGenerics = query?.Generics
        .Select(item => (item.Key, string.Join(";", item.Value.constraints.Select(constraint => constraint.DisplayName).OrderBy(Linq.XtoX))))
        .OrderBy(key => key.Key);

      var expectedGenerics = generics
        .Select(item => (item.Key, string.Join(";", item.Value.constraints.OrderBy(Linq.XtoX))))
        .OrderBy(key => key.Key);

      Assert.Equal(expectedGenerics, actualGenerics);
    }

    [Theory]
    [Trait("Category", nameof(IClass))]
    [Trait("Category", nameof(IEvent))]
    [MemberData(nameof(GetClassInheritedEventsData))]
    public void ValidateInheritedEventMembers(IResolver resolver, string name, string memberName, string sourceTypeName)
    {
      var query = GetClass(resolver, name);

      var inheritedMember = query?.InheritedTypes.Value.FirstOrDefault(member => member.Key.Name.Equals(memberName)) ?? default;

      Assert.Equal(sourceTypeName, inheritedMember.Value.Name);
    }

    [Theory]
    [Trait("Category", nameof(IClass))]
    [Trait("Category", nameof(IDelegate))]
    [MemberData(nameof(GetClassInheritedDelegatesData))]
    public void ValidateInheritedDelegateMembers(IResolver resolver, string name, string memberName, string sourceTypeName)
    {
      var query = GetClass(resolver, name);

      var inheritedMember = query?.InheritedTypes.Value.FirstOrDefault(member => member.Key.Name.Equals(memberName)) ?? default;

      Assert.Equal(sourceTypeName, inheritedMember.Value.Name);
    }

    [Theory]
    [Trait("Category", nameof(IClass))]
    [Trait("Category", nameof(IProperty))]
    [MemberData(nameof(GetClassInheritedPropertiesData))]
    public void ValidateInheritedPropertyMembers(IResolver resolver, string name, string memberName, string sourceTypeName)
    {
      var query = GetClass(resolver, name);

      var inheritedMember = query?.InheritedTypes.Value.FirstOrDefault(member => member.Key.Name.Equals(memberName)) ?? default;

      Assert.Equal(sourceTypeName, inheritedMember.Value.Name);
    }

    [Theory]
    [Trait("Category", nameof(IClass))]
    [Trait("Category", nameof(IMethod))]
    [MemberData(nameof(GetClassInheritedMethodsData))]
    public void ValidateInheritedMethodMembers(IResolver resolver, string name, string memberName, string sourceTypeName)
    {
      var query = GetClass(resolver, name);

      var inheritedMember = query?.InheritedTypes.Value.FirstOrDefault(member => member.Key.Name.Equals(memberName)) ?? default;

      Assert.Equal(sourceTypeName, inheritedMember.Value.Name);
    }

    [Theory]
    [Trait("Category", nameof(IEnum))]
    [MemberData(nameof(GetClassWithMembersData))]
    public void ValidateClassNestedTypeEnum(IResolver resolver, string name)
    {
      var query = GetClass(resolver, name);

      var enumType = query?.NestedTypes.OfType<IEnum>().FirstOrDefault(nested => nested.Name.Equals("MyEnum"));

      Assert.False(enumType is null, $"{resolver.GetType().FullName}: The '{name}' class is missing the expected nested enum.");
    }

    [Theory]
    [Trait("Category", nameof(IEnum))]
    [MemberData(nameof(GetClassWithMembersData))]
    public void ValidateClassNestedTypeEnumCount(IResolver resolver, string name)
    {
      var query = GetClass(resolver, name);

      var enumCount = query?.NestedTypes.OfType<IEnum>().Count() ?? 0;

      Assert.True(enumCount == 1, $"{resolver.GetType().FullName}: The '{name}' class has an unexpected number of nested enums.");
    }

    [Theory]
    [Trait("Category", nameof(IStruct))]
    [MemberData(nameof(GetClassWithMembersData))]
    public void ValidateClassNestedTypeStruct(IResolver resolver, string name)
    {
      var query = GetClass(resolver, name);

      var structType = query?.NestedTypes.OfType<IStruct>().FirstOrDefault(nested => nested.Name.Equals("MyStruct"));

      Assert.False(structType is null, $"{resolver.GetType().FullName}: The '{name}' class is missing the expected nested struct.");
    }

    [Theory]
    [Trait("Category", nameof(IStruct))]
    [MemberData(nameof(GetClassWithMembersData))]
    public void ValidateClassNestedTypeStructCount(IResolver resolver, string name)
    {
      var query = GetClass(resolver, name);

      var structCount = query?.NestedTypes.OfType<IStruct>().Count() ?? 0;

      Assert.True(structCount == 1, $"{resolver.GetType().FullName}: The '{name}' class has an unexpected number of nested structs.");
    }

    [Theory]
    [Trait("Category", nameof(IClass))]
    [MemberData(nameof(GetClassWithMembersData))]
    public void ValidateClassNestedTypeClass(IResolver resolver, string name)
    {
      var query = GetClass(resolver, name);

      var classType = query?.NestedTypes.OfType<IClass>().FirstOrDefault(nested => nested.Name.Equals("MyClass"));

      Assert.False(classType is null, $"{resolver.GetType().FullName}: The '{name}' class is missing the expected nested class.");
    }

    [Theory]
    [Trait("Category", nameof(IClass))]
    [MemberData(nameof(GetClassWithMembersData))]
    public void ValidateClassNestedTypeClassCount(IResolver resolver, string name)
    {
      var query = GetClass(resolver, name);

      var classCount = query?.NestedTypes.OfType<IClass>().Count() ?? 0;

      Assert.True(classCount == 1, $"{resolver.GetType().FullName}: The '{name}' class has an unexpected number of nested classes.");
    }

    [Theory]
    [Trait("Category", nameof(IClass))]
    [MemberData(nameof(GetClassWithMembersData))]
    public void ValidateClassNestedTypeInterface(IResolver resolver, string name)
    {
      var query = GetClass(resolver, name);

      var interfaceType = query?.NestedTypes.OfType<IInterface>().FirstOrDefault(nested => nested.Name.Equals("IMyInterface"));

      Assert.False(interfaceType is null, $"{resolver.GetType().FullName}: The '{name}' class is missing the expected nested interface.");
    }

    [Theory]
    [Trait("Category", nameof(IClass))]
    [MemberData(nameof(GetClassWithMembersData))]
    public void ValidateClassNestedTypeInterfaceCount(IResolver resolver, string name)
    {
      var query = GetClass(resolver, name);

      var interfaceCount= query?.NestedTypes.OfType<IInterface>().Count(item => !(item is IClass || item is IStruct)) ?? 0;

      Assert.True(interfaceCount == 1, $"{resolver.GetType().FullName}: The '{name}' class has an unexpected number of nested interfaces.");
    }
  }
}
