# IMemberSettings `interface`

## Description
Interface for [IResolver](./IResolver.md) settings

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph ModularDoc.Members
  ModularDoc.Members.IMemberSettings[[IMemberSettings]]
  class ModularDoc.Members.IMemberSettings interfaceStyle;
  end
  subgraph ModularDoc
  ModularDoc.ILibrarySettings[[ILibrarySettings]]
  class ModularDoc.ILibrarySettings interfaceStyle;
  end
ModularDoc.ILibrarySettings --> ModularDoc.Members.IMemberSettings
```

## Members
### Properties
#### Public  properties
| Type | Name | Methods |
| --- | --- | --- |
| `IReadOnlyCollection`&lt;`string`&gt; | [`Paths`](#paths)<br>Paths to member sources | `get` |

## Details
### Summary
Interface for [IResolver](./IResolver.md) settings

### Inheritance
 - [
`ILibrarySettings`
](../ILibrarySettings.md)

### Properties
#### Paths
```csharp
public IReadOnlyCollection<string> Paths { get; }
```
##### Summary
Paths to member sources

*Generated with* [*ModularDoc*](https://github.com/hailstorm75/ModularDoc)
