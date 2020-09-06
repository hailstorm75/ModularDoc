using MarkDoc.Members.Members;
using MarkDoc.Members.Types;
using MarkDoc.Members;
using System.Collections.Generic;
using System.Linq;
using System;
using UT.Members.Data;
using Xunit;

namespace UT.Members.MemberTests
{
  public class PropertyTests
  {
    #region Data provider

    private static IEnumerable<object[]> GetPropertyGettersSettersData()
    {
      foreach (var container in new ResolversProvider())
      {
        var resolver = container.First() as IResolver;
        resolver?.Resolve(Constants.TEST_ASSEMBLY);

        var types = resolver?.Types.Value["TestLibrary.Members.Properties"].OfType<IInterface>();
        foreach (var type in types ?? throw new Exception())
        {
          yield return new object[] { type, Constants.PROPERTY_GET_SET, true, true };
          yield return new object[] { type, Constants.PROPERTY_GET, true, false };
          yield return new object[] { type, Constants.PROPERTY_SET, false, true };
        }
      }
    }

    #endregion

    [Theory]
    [Trait("Category", nameof(IProperty))]
    [MemberData(nameof(GetPropertyGettersSettersData))]
    public void ValidatePropertyGettersSetters(IInterface type, string name, bool hasGet, bool hasSet)
    {
      var query = type.Properties.First(property => property.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));

      Assert.True((query.GetAccessor != null) == hasGet && (query.SetAccessor != null) == hasSet);
    }
  }
}
