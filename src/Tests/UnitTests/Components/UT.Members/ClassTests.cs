using System.Collections.Generic;
using System.Linq;
using MarkDoc.Members;
using MarkDoc.Members.Enums;
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

    #endregion

    private static IClass? GetClass(IResolver resolver, string name)
    {
      resolver.Resolve(Constants.TEST_ASSEMBLY);

      return resolver
        .GetTypes<IClass>()
        .FirstOrDefault(type => type.Name.Equals(name));
    }

    [Theory]
    [Trait("Category", nameof(IClass))]
    [MemberData(nameof(GetClassNamesData))]
    public void ValidateInterfaceNames(IResolver resolver, string name)
    {
      var query = GetClass(resolver, name);

      Assert.NotNull(query);
    }

    [Theory]
    [Trait("Category", nameof(IClass))]
    [MemberData(nameof(GetClassAccessorsData))]
    public void ValidateInterfaceAccessors(IResolver resolver, string name, AccessorType accessor)
    {
      var query = GetClass(resolver, name);

      Assert.True(query?.Accessor == accessor, $"{resolver.GetType().FullName}: The '{name}' accessor type is invalid. Expected '{accessor}' != Actual '{query?.Accessor}'");
    }
  }
}
