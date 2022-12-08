# IDiagramResolver `interface`

## Description
Interface for diagram resolvers

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph ModularDoc.Diagrams
  ModularDoc.Diagrams.IDiagramResolver[[IDiagramResolver]]
  class ModularDoc.Diagrams.IDiagramResolver interfaceStyle;
  end
```

## Members
### Methods
#### Public  methods
| Returns | Name |
| --- | --- |
| `bool` | [`TryGenerateDiagram`](#trygeneratediagram)([`IType`](../members/types/IType.md) type, out (`string` name, `string` content) diagram) |

## Details
### Summary
Interface for diagram resolvers

### Methods
#### TryGenerateDiagram
```csharp
public bool TryGenerateDiagram(IType type, out (string name, string content) diagram)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| [`IType`](../members/types/IType.md) | type |   |
| `out` (`string` name, `string` content) | diagram |   |

*Generated with* [*ModularDoc*](https://github.com/hailstorm75/ModularDoc)
