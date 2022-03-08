using MarkDoc.Elements;
using MarkDoc.Members.Types;

namespace MarkDoc.Generator
{
  /// <summary>
  /// Interface for type printers
  /// </summary>
  public interface ITypeComposer
  {
    /// <summary>
    /// Composes a <see cref="IPage"/> from the provided <paramref name="type"/>
    /// </summary>
    /// <param name="type">Type to process</param>
    /// <returns>Composed page</returns>
    IPage Compose(IType type);

    /// <summary>
    /// Composes a <see cref="IPage"/> containing links to all types
    /// </summary>
    /// <returns>Composed page</returns>
    IPage ComposeTableOfContents();
  }
}
