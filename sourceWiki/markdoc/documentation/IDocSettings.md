# IDocSettings `interface`

## Description
Interface for [IDocResolver](./IDocResolver.md) settings

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
| `IReadOnlyCollection`&lt;`string`&gt; | [`Paths`](#paths)<br>Paths to documentation sources | `get, set` |

## Details
### Summary
Interface for [IDocResolver](./IDocResolver.md) settings

### Inheritance
 - [
`ILibrarySettings`
](../core/ILibrarySettings.md)

### Properties
#### Paths
```csharp
public IReadOnlyCollection<string> Paths { get; set; }
```
##### Summary
Paths to documentation sources

*Generated with* [*MarkDoc*](https://github.com/hailstorm75/MarkDoc.Core)
