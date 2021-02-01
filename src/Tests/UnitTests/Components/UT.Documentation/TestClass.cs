using System.Collections.Generic;
using System.IO;
using System.Linq;
using MarkDoc.Documentation;
using UT.Documentation.Data;
using Xunit;

namespace UT.Documentation
{
  public class TestClass
  {
    public static IEnumerable<object[]> TestData()
      => new ResolversProvider().Select(resolver => new[] {resolver});

    [Theory]
    [MemberData(nameof(TestData))]
    public async void TestMethod(IDocResolver resolver)
      => await Assert.ThrowsAsync<FileNotFoundException>(() => resolver.ResolveAsync("path")).ConfigureAwait(false);
  }
}
