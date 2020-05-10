using MarkDoc.Members;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MarkDoc.Documentation
{
  public interface IDocResolver
  {
    /// <summary>
    /// Resolve xml documentation on given <paramref name="path"/>
    /// </summary>
    /// <param name="path">Path to documentation</param>
    Task Resolve(string path);

    bool TryFindType(IType type, out IDocElement? resultType, out IReadOnlyDictionary<string, IDocElement>? resultMembers);
  }
}
