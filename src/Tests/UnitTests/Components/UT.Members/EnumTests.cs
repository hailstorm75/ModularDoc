using MarkDoc.Members;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using MarkDoc.Members.Enums;
using MarkDoc.Members.Types;
using UT.Members.Data;
using Xunit;

namespace UT.Members
{
  public class EnumTests
  {
    private static IEnumerable<object[]> GetEnumAccessorsData()
    {
      var data = new []
      {
        (Constants.PUBLIC_ENUM, AccessorType.Public),
        (Constants.INTERNAL_ENUM, AccessorType.Internal),
        (Constants.PUBLIC_NESTED_ENUM, AccessorType.Public),
        (Constants.PROTECTED_NESTED_ENUM, AccessorType.Protected),
        (Constants.INTERNAL_NESTED_ENUM, AccessorType.Internal),
      };

      foreach (var (name, accessor) in data)
        foreach (var resolver in new ResolversProvider())
          yield return new[] { resolver.First(), name, accessor };
    }

    private static IEnumerable<object[]> GetEnumFieldsData()
    {
      var data = new []
      {
        Constants.PUBLIC_ENUM,
        Constants.INTERNAL_ENUM,
        Constants.PUBLIC_NESTED_ENUM,
        Constants.PROTECTED_NESTED_ENUM,
        Constants.INTERNAL_NESTED_ENUM,
      };

      var fields = new[] { "FieldA", "FieldB" };
      foreach (var name in data)
        foreach (var resolver in new ResolversProvider())
          yield return new[] { resolver.First(), name, fields };
    }

    [Theory]
    [Category(nameof(IEnum))]
    [MemberData(nameof(GetEnumAccessorsData))]
    public void ValidateEnumAccessors(IResolver resolver, string name, AccessorType accessor)
    {
      resolver.Resolve(Constants.TEST_ASSEMBLY);

      var query = resolver
        .GetTypes<IEnum>()
        .FirstOrDefault(type => type.Name.Equals(name));

      Assert.True(query?.Accessor == accessor, $"{resolver.GetType().FullName}: The '{name}' accessor type is invalid. Expected '{accessor}' != Actual '{query?.Accessor}'");
    }

    [Theory]
    [Category(nameof(IEnum))]
    [MemberData(nameof(GetEnumFieldsData))]
    public void ValidateEnumFieldNames(IResolver resolver, string name, string[] fields)
    {
      resolver.Resolve(Constants.TEST_ASSEMBLY);

      var query = resolver
        .GetTypes<IEnum>()
        .FirstOrDefault(type => type.Name.Equals(name));

      Assert.Equal(query?.Fields.Select(field => field.Name), fields);
    }

    [Theory]
    [Category(nameof(IEnum))]
    [MemberData(nameof(GetEnumFieldsData))]
#pragma warning disable xUnit1026 // Theory methods should use all of their parameters
    public void ValidateEnumFieldAreStatic(IResolver resolver, string name, string[] fields)
#pragma warning restore xUnit1026 // Theory methods should use all of their parameters
    {
      resolver.Resolve(Constants.TEST_ASSEMBLY);

      var query = resolver
        .GetTypes<IEnum>()
        .FirstOrDefault(type => type.Name.Equals(name));

      Assert.True(query?.Fields.Select(field => field.IsStatic).Any());
    }
  }
}
