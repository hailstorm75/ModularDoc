# ITypeComposer `interface`

## Description
Interface for type printers

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph ModularDoc.Composer
  ModularDoc.Composer.ITypeComposer[[ITypeComposer]]
  class ModularDoc.Composer.ITypeComposer interfaceStyle;
  end
```

## Members
### Methods
#### Public  methods
| Returns | Name |
| --- | --- |
| [`IPage`](../elements/IPage.md) | [`Compose`](#compose)([`IType`](../members/types/IType.md) type)<br>Prints a [IPage](../elements/IPage.md) from the provided `type` |

## Details
### Summary
Interface for type printers

### Methods
#### Compose
```csharp
public IPage Compose(IType type)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| [`IType`](../members/types/IType.md) | type | Type to process |

##### Summary
Prints a [IPage](../elements/IPage.md) from the provided `type`

##### Returns
Generated page

*Generated with* [*ModularDoc*](https://github.com/hailstorm75/ModularDoc)
