# IDiagram `interface`

## Description
Interface for diagram elements

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph MarkDoc.Elements
  MarkDoc.Elements.IDiagram[[IDiagram]]
  class MarkDoc.Elements.IDiagram interfaceStyle;
  MarkDoc.Elements.IElement[[IElement]]
  class MarkDoc.Elements.IElement interfaceStyle;
  end
MarkDoc.Elements.IElement --> MarkDoc.Elements.IDiagram
```

## Members
### Methods
#### Public  methods
| Returns | Name |
| --- | --- |
| `ValueTask` | [`ToExternalFile`](#toexternalfile)(`string` directory)<br>Exports the diagram to an external file |

## Details
### Summary
Interface for diagram elements

### Inheritance
 - [
`IElement`
](./IElement.md)

### Methods
#### ToExternalFile
```csharp
public ValueTask ToExternalFile(string directory)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `string` | directory | Path to the directory to export to |

##### Summary
Exports the diagram to an external file

*Generated with* [*MarkDoc*](https://github.com/hailstorm75/MarkDoc.Core)
