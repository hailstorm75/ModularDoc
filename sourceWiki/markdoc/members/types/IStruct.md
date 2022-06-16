# IStruct `interface`

## Description
Interface for struct types

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph MarkDoc.Members.Types
  MarkDoc.Members.Types.IStruct[[IStruct]]
  class MarkDoc.Members.Types.IStruct interfaceStyle;
  MarkDoc.Members.Types.IInterface[[IInterface]]
  class MarkDoc.Members.Types.IInterface interfaceStyle;
  MarkDoc.Members.Types.IType[[IType]]
  class MarkDoc.Members.Types.IType interfaceStyle;
  end
MarkDoc.Members.Types.IInterface --> MarkDoc.Members.Types.IStruct
MarkDoc.Members.Types.IType --> MarkDoc.Members.Types.IInterface
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
public abstract IReadOnlyCollection<IConstructor> Constructors { get; }
```
##### Summary
Struct constructors

#### IsReadOnly
```csharp
public abstract bool IsReadOnly { get; }
```
##### Summary
Determines whether the struct is readonly

*Generated with* [*MarkDoc*](https://github.com/hailstorm75/MarkDoc.Core)
