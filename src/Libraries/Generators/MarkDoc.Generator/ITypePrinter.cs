using MarkDoc.Elements;
using MarkDoc.Members;

namespace MarkDoc.Generator
{
  /// <summary>
  /// Interface for type printers
  /// </summary>
  public interface ITypePrinter
  {
    /// <summary>
    /// Prints a <see cref="IPage"/> from the provided <paramref name="type"/>
    /// </summary>
    /// <param name="type">Type to process</param>
    /// <returns>Generated page</returns>
    IPage Print(IInterface type);
  }
}
