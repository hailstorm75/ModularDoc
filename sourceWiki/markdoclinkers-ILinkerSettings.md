# ILinkerSettings `interface`

## Description
Interface for [ILinker](./markdoclinkers-ILinker) object settings

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph MarkDoc.Linkers
  MarkDoc.Linkers.ILinkerSettings[[ILinkerSettings]]
  class MarkDoc.Linkers.ILinkerSettings interfaceStyle;
  end
  subgraph MarkDoc.Core
  MarkDoc.Core.ILibrarySettings[[ILibrarySettings]]
  class MarkDoc.Core.ILibrarySettings interfaceStyle;
  end
MarkDoc.Core.ILibrarySettings --> MarkDoc.Linkers.ILinkerSettings
```

## Details
### Summary
Interface for [ILinker](./markdoclinkers-ILinker) object settings

### Inheritance
 - [
`ILibrarySettings`
](./markdoccore-ILibrarySettings)

*Generated with* [*MarkDoc*](https://github.com/hailstorm75/MarkDoc.Core)
