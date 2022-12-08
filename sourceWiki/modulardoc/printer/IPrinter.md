# IPrinter `interface`

## Description
Interface for documentation printers

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph ModularDoc.Printer
  ModularDoc.Printer.IPrinter[[IPrinter]]
  class ModularDoc.Printer.IPrinter interfaceStyle;
  end
```

## Members
### Methods
#### Public  methods
| Returns | Name |
| --- | --- |
| `Task` | [`Print`](#print)(`IEnumerable`&lt;[`IType`](../members/types/IType.md)&gt; types, `string` path) |

## Details
### Summary
Interface for documentation printers

### Methods
#### Print
```csharp
public Task Print(IEnumerable<IType> types, string path)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `IEnumerable`&lt;[`IType`](../members/types/IType.md)&gt; | types |   |
| `string` | path |   |

*Generated with* [*ModularDoc*](https://github.com/hailstorm75/ModularDoc)
