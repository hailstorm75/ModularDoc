namespace ModularDoc.Generator.Basic

open ModularDoc.Diagrams
open ModularDoc.Linkers
open ModularDoc.Members
open ModularDoc.Elements
open ModularDoc.Documentation

/// <summary>
/// Tools for composing types
/// </summary>
type Tools = {
  /// <summary>
  /// Type linker
  /// </summary>
  linker: ILinker
  /// <summary>
  /// Documentation elements creator
  /// </summary>
  creator: IElementCreator;
  /// <summary>
  /// Documentation resolver
  /// </summary>
  docResolver: IDocResolver;
  /// <summary>
  /// Type resolver
  /// </summary>
  typeResolver: IResolver
  /// <summary>
  /// Diagram resolver
  /// </summary>
  diagramResolver: IDiagramResolver
}
