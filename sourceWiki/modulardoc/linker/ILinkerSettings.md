# ILinkerSettings `interface`

## Description
Interface for [ILinker](./ILinker.md) object settings

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph ModularDoc.Linker
  ModularDoc.Linker.ILinkerSettings[[ILinkerSettings]]
  class ModularDoc.Linker.ILinkerSettings interfaceStyle;
  end
  subgraph ModularDoc.Core
  ModularDoc.Core.ILibrarySettings[[ILibrarySettings]]
  class ModularDoc.Core.ILibrarySettings interfaceStyle;
  end
ModularDoc.Core.ILibrarySettings --> ModularDoc.Linker.ILinkerSettings
```

## Details
### Summary
Interface for [ILinker](./ILinker.md) object settings

### Inheritance
 - [
`ILibrarySettings`
](../core/ILibrarySettings.md)

*Generated with* [*ModularDoc*](https://github.com/hailstorm75/ModularDoc)
