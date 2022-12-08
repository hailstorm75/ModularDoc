# IEnum `interface`

## Description
Interface for enums

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph ModularDoc.Members.Types
  ModularDoc.Members.Types.IEnum[[IEnum]]
  class ModularDoc.Members.Types.IEnum interfaceStyle;
  ModularDoc.Members.Types.IType[[IType]]
  class ModularDoc.Members.Types.IType interfaceStyle;
  end
ModularDoc.Members.Types.IType --> ModularDoc.Members.Types.IEnum
```

## Members
### Properties
#### Public  properties
| Type | Name | Methods |
| --- | --- | --- |
| `IReadOnlyCollection`&lt;[`IEnumField`](../members/IEnumField.md)&gt; | [`Fields`](#fields)<br>Enum fields | `get` |

## Details
### Summary
Interface for enums

### Inheritance
 - [
`IType`
](./IType.md)

### Properties
#### Fields
```csharp
public IReadOnlyCollection<IEnumField> Fields { get; }
```
##### Summary
Enum fields

*Generated with* [*ModularDoc*](https://github.com/hailstorm75/ModularDoc)
