# IResArray `interface`

## Description
Interface for resolved arrays

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph MarkDoc.Members.ResolvedTypes
  MarkDoc.Members.ResolvedTypes.IResArray[[IResArray]]
  class MarkDoc.Members.ResolvedTypes.IResArray interfaceStyle;
  end
```

## Members
### Properties
#### Public  properties
| Type | Name | Methods |
| --- | --- | --- |
| [`IResType`](./markdocmembersresolvedtypes-IResType.md) | [`ArrayType`](markdocmembersresolvedtypes-IResArray.md#arraytype)<br>Type of given array | `get` |
| `int` | [`Dimension`](markdocmembersresolvedtypes-IResArray.md#dimension)<br>Dimension of array | `get` |
| `bool` | [`IsJagged`](markdocmembersresolvedtypes-IResArray.md#isjagged)<br>Determines whether the array is a jagged array type | `get` |

## Details
### Summary
Interface for resolved arrays

### Properties
#### ArrayType
```csharp
public abstract IResType ArrayType { get }
```
##### Summary
Type of given array

#### IsJagged
```csharp
public abstract bool IsJagged { get }
```
##### Summary
Determines whether the array is a jagged array type

#### Dimension
```csharp
public abstract int Dimension { get }
```
##### Summary
Dimension of array

*Generated with* [*MarkDoc*](https://github.com/hailstorm75/MarkDoc.Core)
