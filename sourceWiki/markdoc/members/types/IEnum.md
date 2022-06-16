# IEnum `interface`

## Description
Interface for enums

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph MarkDoc.Members.Types
  MarkDoc.Members.Types.IEnum[[IEnum]]
  class MarkDoc.Members.Types.IEnum interfaceStyle;
  MarkDoc.Members.Types.IType[[IType]]
  class MarkDoc.Members.Types.IType interfaceStyle;
  end
MarkDoc.Members.Types.IType --> MarkDoc.Members.Types.IEnum
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
public abstract IReadOnlyCollection<IEnumField> Fields { get; }
```
##### Summary
Enum fields

*Generated with* [*MarkDoc*](https://github.com/hailstorm75/MarkDoc.Core)
