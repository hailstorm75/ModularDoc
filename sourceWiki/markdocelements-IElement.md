# IElement `interface`

## Description
Interface for elements

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph MarkDoc.Elements
  MarkDoc.Elements.IElement[[IElement]]
  class MarkDoc.Elements.IElement interfaceStyle;
  end
```

## Members
### Methods
#### Public  methods
| Returns | Name |
| --- | --- |
| `IEnumerable` | [`Print`](markdocelements-IElement#print)()<br>Converts given element to a sequence of strings |

## Details
### Summary
Interface for elements

### Methods
#### Print
```csharp
public abstract IEnumerable Print()
```
##### Summary
Converts given element to a sequence of strings

##### Returns
Strings to export

*Generated with* [*MarkDoc*](https://github.com/hailstorm75/MarkDoc.Core)
