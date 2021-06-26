using System.Collections.Generic;
using MarkDoc.Members;
using MarkDoc.Members.Types;
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

      Assert.True(query?.RawName.Equals($"{expected}.{name}"), $"{resolver.GetType().FullName}: The '{name}' raw name is invalid. Expected '{expected}.{name}' != Actual '{query?.RawName}'.");
    }

    // [Theory]
    // [Trait("Category", nameof(IRecord))]
    // [MemberData(nameof(GetClassAccessorsData))]
    // public void ValidateRecordAccessors(IResolver resolver, string name, AccessorType accessor)
    // {
    //   
    // }
    //
    // public void ValidateRecordAbstract(IResolver resolver, string name, bool expected)
    // {
    //   
    // }
    //
    // public void ValidateRecordSealed(IResolver resolver, string name, bool expected)
    // {
    //   
    // }
    //
    // public void ValidateRecordNamespace(IResolver resolver, string name, string expected)
    // {
    //   
    // }
    //
    // public void ValidateRecordHasBase(IResolver resolver, string name, bool expected)
    // {
    //   
    // }
  }
}