using MarkDoc.Members.ResolvedTypes;
using MarkDoc.Members.Types;

namespace MarkDoc.Linkers
{
  /// <summary>
  /// Interface for creating links
  /// </summary>
  public interface ILinker
  {
    /// <summary>
    /// Creates a link to a given <paramref name="type"/>
    /// </summary>
    /// <param name="type">Type to link to</param>
    /// <returns>Retrieved link</returns>
    string CreateLink(IResType type);

    /// <summary>
    /// Creates a link to a given <paramref name="type"/>
    /// </summary>
    /// <param name="type">Type to link to</param>
    /// <returns>Retrieved link</returns>
    string CreateLink(IType type);
  }
}
