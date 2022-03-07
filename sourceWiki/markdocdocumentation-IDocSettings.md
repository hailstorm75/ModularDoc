# IDocSettings `interface`

## Description
Interface for [IDocResolver](./markdocdocumentation-IDocResolver) settings

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph MarkDoc.Documentation
  MarkDoc.Documentation.IDocSettings[[IDocSettings]]
  class MarkDoc.Documentation.IDocSettings interfaceStyle;
  end
  subgraph MarkDoc.Core
  MarkDoc.Core.ILibrarySettings[[ILibrarySettings]]
  class MarkDoc.Core.ILibrarySettings interfaceStyle;
  end
MarkDoc.Core.ILibrarySettings --> MarkDoc.Documentation.IDocSettings
```

## Members
### Properties
#### Public  properties
| Type | Name | Methods |
| --- | --- | --- |
| `IReadOnlyCollection`&lt;`string`&gt; | [`Paths`](markdocdocumentation-IDocSettings#paths)<br>Paths to documentation sources | `get; set` |

## Details
### Summary
Interface for [IDocResolver](./markdocdocumentation-IDocResolver) settings

### Inheritance
 - [
`ILibrarySettings`
](./markdoccore-ILibrarySettings)

### Properties
#### Paths
```csharp
public abstract IReadOnlyCollection Paths { get; set }
```
##### Summary
Paths to documentation sources

*Generated with* [*MarkDoc*](https://github.com/hailstorm75/MarkDoc.Core)
