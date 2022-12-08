using System.Collections.Generic;
using ModularDoc.Members;
using ModularDoc.Members.Enums;
using ModularDoc.Members.Types;
using UT.Members.Data;
using Xunit;

namespace UT.Members.TypeTests
{
  public class RecordTests
  {
    #region Data providers

    public static IEnumerable<object?[]> GetRecordNamesData()
    {
      var data = new[]
      {
        Constants.PUBLIC_RECORD,
        Constants.INTERNAL_RECORD,
        Constants.PUBLIC_NESTED_RECORD,
        Constants.PROTECTED_NESTED_RECORD,
        Constants.PROTECTED_INTERNAL_NESTED_RECORD,
        Constants.INTERNAL_NESTED_RECORD,
        Constants.PUBLIC_GENERIC_RECORD,
        Constants.PUBLIC_NESTED_GENERIC_RECORD
      };

      return data.ComposeData();
    }

    public static IEnumerable<object[]> GetRecordNamespacesData()
    {
      const string nameSpace = Constants.RECORD_NAMESPACE;

      var data = new[]
      {
        new object[] {Constants.PUBLIC_RECORD, nameSpace},
        new object[] {Constants.INTERNAL_RECORD, nameSpace},
        new object[] {Constants.PUBLIC_NESTED_RECORD, $"{nameSpace}.RecordParent"},
        new object[] {Constants.PROTECTED_NESTED_RECORD, $"{nameSpace}.RecordParent"},
        new object[] {Constants.PROTECTED_INTERNAL_NESTED_RECORD, $"{nameSpace}.RecordParent"},
        new object[] {Constants.INTERNAL_NESTED_RECORD, $"{nameSpace}.RecordParent"}
      };

      return data.ComposeData();
    }

    public static IEnumerable<object[]> GetRecordAccessorsData()
    {
      var data = new[]
      {
        new object[] {Constants.PUBLIC_RECORD, AccessorType.Public},
        new object[] {Constants.INTERNAL_RECORD, AccessorType.Internal},
        new object[] {Constants.PUBLIC_NESTED_RECORD, AccessorType.Public},
        new object[] {Constants.PROTECTED_NESTED_RECORD, AccessorType.Protected},
        new object[] {Constants.PROTECTED_INTERNAL_NESTED_RECORD, AccessorType.ProtectedInternal},
        new object[] {Constants.INTERNAL_NESTED_RECORD, AccessorType.Internal},
      };

      return data.ComposeData();
    }

    public static IEnumerable<object[]> GetRecordAbstractData()
    {
      var data = new[]
      {
        new object[] { Constants.PUBLIC_RECORD, false },
        new object[] { Constants.PUBLIC_RECORD_SEALED, false },
        new object[] { Constants.PUBLIC_RECORD_ABSTRACT, true }
      };

      return data.ComposeData();
    }

    public static IEnumerable<object[]> GetRecordSealedData()
    {
      var data = new[]
      {
        new object[] {Constants.PUBLIC_RECORD, false},
        new object[] {Constants.PUBLIC_RECORD_ABSTRACT, false},
        new object[] {Constants.PUBLIC_RECORD_SEALED, true}
      };

      return data.ComposeData();
    }

    private static IRecord? GetRecord(IResolver resolver, string name)
    {
      resolver.Resolve(Constants.TEST_ASSEMBLY);

      return resolver.FindType<IRecord>(name);
    }

    #endregion

    [Theory]
    [Trait("Category", nameof(IRecord))]
    [MemberData(nameof(GetRecordNamesData))]
    public void ValidateRecordNames(IResolver resolver, string name)
    {
      var query = GetRecord(resolver, name);

      Assert.NotNull(query);
    }

    [Theory]
    [Trait("Category", nameof(IRecord))]
    [MemberData(nameof(GetRecordNamespacesData))]
    public void ValidateRecordRawNames(IResolver resolver, string name, string expected)
    {
      var query = GetRecord(resolver, name);

      Assert.True(query?.RawName.Equals($"{expected}.{name}"),
        $"{resolver.GetType().FullName}: The '{name}' raw name is invalid. Expected '{expected}.{name}' != Actual '{query?.RawName}'.");
    }

    [Theory]
    [Trait("Category", nameof(IRecord))]
    [MemberData(nameof(GetRecordAccessorsData))]
    public void ValidateRecordAccessors(IResolver resolver, string name, AccessorType accessor)
    {
      var query = GetRecord(resolver, name);

      Assert.True(query?.Accessor == accessor,
        $"{resolver.GetType().FullName}: The '{name}' accessor type is invalid. Expected '{accessor}' != Actual '{query?.Accessor}'");
    }

    [Theory]
    [Trait("Category", nameof(IRecord))]
    [MemberData(nameof(GetRecordAbstractData))]
    public void ValidateRecordAbstract(IResolver resolver, string name, bool expected)
    {
      var query = GetRecord(resolver, name);

      Assert.Equal(expected, query?.IsAbstract);
    }

    [Theory]
    [Trait("Category", nameof(IRecord))]
    [MemberData(nameof(GetRecordSealedData))]
    public void ValidateRecordSealed(IResolver resolver, string name, bool expected)
    {
      var query = GetRecord(resolver, name);

      Assert.Equal(expected, query?.IsSealed);
    }
  }
}