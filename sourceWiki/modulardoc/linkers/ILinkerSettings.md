# ILinkerSettings `interface`

## Description
Interface for [ILinker](./ILinker.md) object settings

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph ModularDoc.Linkers
  ModularDoc.Linkers.ILinkerSettings[[ILinkerSettings]]
  class ModularDoc.Linkers.ILinkerSettings interfaceStyle;
  end
  subgraph ModularDoc
  ModularDoc.ILibrarySettings[[ILibrarySettings]]
  class ModularDoc.ILibrarySettings interfaceStyle;
  end
ModularDoc.ILibrarySettings --> ModularDoc.Linkers.ILinkerSettings
```

## Details
### Summary
Interface for [ILinker](./ILinker.md) object settings

### Inheritance
 - [
`ILibrarySettings`
](../ILibrarySettings.md)

*Generated with* [*ModularDoc*](https://github.com/hailstorm75/ModularDoc)
