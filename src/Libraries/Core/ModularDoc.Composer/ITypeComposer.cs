using ModularDoc.Elements;
using ModularDoc.Members.Types;

namespace ModularDoc.Composer
{
  /// <summary>
  /// Interface for type printers
  /// </summary>
  public interface ITypeComposer
  {
    /// <summary>
    /// Prints a <see cref="IPage"/> from the provided <paramref name="type"/>
    /// </summary>
    /// <param name="type">Type to process</param>
    /// <returns>Generated page</returns>
    IPage Compose(IType type);
  }
}
