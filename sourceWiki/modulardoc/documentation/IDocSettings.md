# IDocSettings `interface`

## Description
Interface for [IDocResolver](./IDocResolver.md) settings

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph ModularDoc.Documentation
  ModularDoc.Documentation.IDocSettings[[IDocSettings]]
  class ModularDoc.Documentation.IDocSettings interfaceStyle;
  end
  subgraph ModularDoc.Core
  ModularDoc.Core.ILibrarySettings[[ILibrarySettings]]
  class ModularDoc.Core.ILibrarySettings interfaceStyle;
  end
ModularDoc.Core.ILibrarySettings --> ModularDoc.Documentation.IDocSettings
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

*Generated with* [*ModularDoc*](https://github.com/hailstorm75/ModularDoc)
