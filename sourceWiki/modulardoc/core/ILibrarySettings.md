# ILibrarySettings `interface`

## Description
Interface for settings of libraries

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph ModularDoc.Core
  ModularDoc.Core.ILibrarySettings[[ILibrarySettings]]
  class ModularDoc.Core.ILibrarySettings interfaceStyle;
  end
```

## Members
### Properties
#### Public  properties
| Type | Name | Methods |
| --- | --- | --- |
| `Guid` | [`Id`](#id)<br>Settings id | `get` |

## Details
### Summary
Interface for settings of libraries

### Properties
#### Id
```csharp
public Guid Id { get; }
```
##### Summary
Settings id

*Generated with* [*ModularDoc*](https://github.com/hailstorm75/ModularDoc)
