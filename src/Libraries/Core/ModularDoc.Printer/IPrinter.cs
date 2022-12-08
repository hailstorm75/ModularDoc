using ModularDoc.Members.Types;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ModularDoc.Printer
{
  /// <summary>
  /// Interface for documentation printers
  /// </summary>
  public interface IPrinter
  {
    /// <summary>
    /// Prints the <paramref name="types"/> to the specified <paramref name="path"/>
    /// </summary>
    /// <param name="types">Types to print</param>
    /// <param name="path">Output path</param>
    Task Print(IEnumerable<IType> types, string path);
  }
}
