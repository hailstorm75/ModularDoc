# IResGeneric `interface`

## Description
Interface for generic resolved types

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph ModularDoc.Members.ResolvedTypes
  ModularDoc.Members.ResolvedTypes.IResGeneric[[IResGeneric]]
  class ModularDoc.Members.ResolvedTypes.IResGeneric interfaceStyle;
  ModularDoc.Members.ResolvedTypes.IResType[[IResType]]
  class ModularDoc.Members.ResolvedTypes.IResType interfaceStyle;
  end
ModularDoc.Members.ResolvedTypes.IResType --> ModularDoc.Members.ResolvedTypes.IResGeneric
```

## Members
### Properties
#### Public  properties
| Type | Name | Methods |
| --- | --- | --- |
| `IReadOnlyCollection`&lt;[`IResType`](./IResType.md)&gt; | [`Generics`](#generics)<br>Generic parameter types | `get` |

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
public IReadOnlyCollection<IResType> Generics { get; }
```
##### Summary
Generic parameter types

*Generated with* [*ModularDoc*](https://github.com/hailstorm75/ModularDoc)
