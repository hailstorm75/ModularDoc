# IStruct `interface`

## Description
Interface for struct types

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph ModularDoc.Members.Types
  ModularDoc.Members.Types.IStruct[[IStruct]]
  class ModularDoc.Members.Types.IStruct interfaceStyle;
  ModularDoc.Members.Types.IInterface[[IInterface]]
  class ModularDoc.Members.Types.IInterface interfaceStyle;
  ModularDoc.Members.Types.IType[[IType]]
  class ModularDoc.Members.Types.IType interfaceStyle;
  end
ModularDoc.Members.Types.IInterface --> ModularDoc.Members.Types.IStruct
ModularDoc.Members.Types.IType --> ModularDoc.Members.Types.IInterface
```

## Members
### Properties
#### Public  properties
| Type | Name | Methods |
| --- | --- | --- |
| `IReadOnlyCollection`&lt;[`IConstructor`](../members/IConstructor.md)&gt; | [`Constructors`](#constructors)<br>Struct constructors | `get` |
| `bool` | [`IsReadOnly`](#isreadonly)<br>Determines whether the struct is readonly | `get` |

## Details
### Summary
Interface for struct types

### Inheritance
 - [
`IInterface`
](./IInterface.md)
 - [
`IType`
](./IType.md)

### Properties
#### Constructors
```csharp
public IReadOnlyCollection<IConstructor> Constructors { get; }
```
##### Summary
Struct constructors

#### IsReadOnly
```csharp
public bool IsReadOnly { get; }
```
##### Summary
Determines whether the struct is readonly

*Generated with* [*ModularDoc*](https://github.com/hailstorm75/ModularDoc)
