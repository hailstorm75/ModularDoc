using System;
using System.Collections.Generic;
using System.Linq;
using MarkDoc.Members;
using UT.Members.Data;
using Xunit;
using Constants = UT.Members.Data.Constants;

namespace UT.Members.TypeTests
{
  public class ResolverTests
  {
    public static IEnumerable<object?[]> GetInvalidAssemblyData()
    {
      var data = new[]
      {
        null,
        string.Empty,
        "../../InvalidAssembly.dll"
      };

      return data.ComposeData();
    }

    public static IEnumerable<object[]> GetResolvers()
      => new ResolversProvider().Select(resolver => new object[] { resolver });

    public static IEnumerable<object?[]> GetInvalidResolveTypeData()
    {
      var data = new[]
      {
        null,
        new object()
      };

      return data.ComposeData();
    }

    [Theory]
    [Trait("Category", nameof(IResolver))]
    [MemberData(nameof(GetInvalidAssemblyData))]
    public void ResolverInvalidAssembly(IResolver resolver, string? path)
      => Assert.ThrowsAny<Exception>(() => resolver.Resolve(path!));

    [Theory]
    [Trait("Category", nameof(IResolver))]
    [MemberData(nameof(GetResolvers))]
    public void ResolverAccessTypesBeforeResolve(IResolver resolver)
    {
      var _ = resolver.Types.Value;

      Assert.ThrowsAny<Exception>(() => resolver.Resolve(Constants.TEST_ASSEMBLY));
    }

    [Theory]
    [Trait("Category", nameof(IResolver))]
    [MemberData(nameof(GetResolvers))]
    public void ResolverFindType(IResolver resolver)
    {
      resolver.Resolve(Constants.TEST_ASSEMBLY);

      Assert.ThrowsAny<Exception>(() => resolver.TryFindType(null!, out var _));
    }
  }
}
