# IResGeneric `interface`

## Description
Interface for generic resolved types

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph MarkDoc.Members.ResolvedTypes
  MarkDoc.Members.ResolvedTypes.IResGeneric[[IResGeneric]]
  class MarkDoc.Members.ResolvedTypes.IResGeneric interfaceStyle;
  MarkDoc.Members.ResolvedTypes.IResType[[IResType]]
  class MarkDoc.Members.ResolvedTypes.IResType interfaceStyle;
  end
MarkDoc.Members.ResolvedTypes.IResType --> MarkDoc.Members.ResolvedTypes.IResGeneric
```

## Members
### Properties
#### Public  properties
| Type | Name | Methods |
| --- | --- | --- |
| `IReadOnlyCollection`&lt;[`IResType`](./IResType.md)&gt; | [`Generics`](markdoc/members/resolvedtypes/IResGeneric.md#generics)<br>Generic parameter types | `get` |

## Details
### Summary
Interface for generic resolved types

### Inheritance
 - [
`IResType`
](./IResType.md)

### Properties
#### Generics
```csharp
public abstract IReadOnlyCollection<IResType> Generics { get; }
```
##### Summary
Generic parameter types

*Generated with* [*MarkDoc*](https://github.com/hailstorm75/MarkDoc.Core)
