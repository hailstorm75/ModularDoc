# IResTuple `interface`

## Description
Interface for resolved tuples

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph ModularDoc.Members.ResolvedTypes
  ModularDoc.Members.ResolvedTypes.IResTuple[[IResTuple]]
  class ModularDoc.Members.ResolvedTypes.IResTuple interfaceStyle;
  ModularDoc.Members.ResolvedTypes.IResType[[IResType]]
  class ModularDoc.Members.ResolvedTypes.IResType interfaceStyle;
  end
ModularDoc.Members.ResolvedTypes.IResType --> ModularDoc.Members.ResolvedTypes.IResTuple
```

## Members
### Properties
#### Public  properties
| Type | Name | Methods |
| --- | --- | --- |
| `IReadOnlyCollection`&lt;(`string` name, [`IResType`](./IResType.md) type)&gt; | [`Fields`](#fields)<br>Tuple fields | `get` |
| `bool` | [`IsValueTuple`](#isvaluetuple)<br>Determines whether the tuple is a value tuple | `get` |

## Details
### Summary
Interface for resolved tuples

### Inheritance
 - [
`IResType`
](./IResType.md)

### Properties
#### Fields
```csharp
public IReadOnlyCollection<(string name, IResType type)> Fields { get; }
```
##### Summary
Tuple fields

#### IsValueTuple
```csharp
public bool IsValueTuple { get; }
```
##### Summary
Determines whether the tuple is a value tuple

*Generated with* [*ModularDoc*](https://github.com/hailstorm75/ModularDoc)
