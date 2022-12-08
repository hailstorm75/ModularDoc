using System.Threading.Tasks;

namespace ModularDoc.Elements
{
  /// <summary>
  /// Interface for diagram elements
  /// </summary>
  public interface IDiagram
    : IElement
  {
    /// <summary>
    /// Exports the diagram to an external file
    /// </summary>
    /// <param name="directory">Path to the directory to export to</param>
    ValueTask ToExternalFile(string directory);
  }
}