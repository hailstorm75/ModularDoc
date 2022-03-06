using MarkDoc.Members.Types;

namespace MarkDoc.Diagrams
{
  /// <summary>
  /// Interface for diagram resolvers
  /// </summary>
  public interface IDiagramResolver
  {
    /// <summary>
    /// Attempts to generate a diagram for the given <paramref name="type"/>
    /// </summary>
    /// <param name="type">Type to process</param>
    /// <param name="diagram">Resulting diagram</param>
    /// <returns>Success if diagram generated</returns>
    bool TryGenerateDiagram(IType type, out (string name, string content) diagram);
  }
}