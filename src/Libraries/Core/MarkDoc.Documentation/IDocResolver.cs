using System.Threading.Tasks;
using MarkDoc.Members.Types;

namespace MarkDoc.Documentation
{
  /// <summary>
  /// Interface for documentation resolvers
  /// </summary>
  public interface IDocResolver
  {
    /// <summary>
    /// Resolve xml documentation on given <paramref name="path"/>
    /// </summary>
    /// <param name="path">Path to documentation</param>
    Task Resolve(string path);

    /// <summary>
    /// Tries to find a given <paramref name="type"/>
    /// </summary>
    /// <param name="type">Type to find</param>
    /// <param name="resultType">Search result</param>
    /// <returns>True if found</returns>
    bool TryFindType(IType type, out IDocElement? resultType);
  }
}
