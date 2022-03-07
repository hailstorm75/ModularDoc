# IType `interface`

## Description
Interface for types

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph MarkDoc.Members.Types
  MarkDoc.Members.Types.IType[[IType]]
  class MarkDoc.Members.Types.IType interfaceStyle;
  end
```

## Members
### Properties
#### Public  properties
| Type | Name | Methods |
| --- | --- | --- |
| [`AccessorType`](../enums/AccessorType.md) | [`Accessor`](markdoc/members/types/IType.md#accessor)<br>Type accessor | `get` |
| `bool` | [`IsNested`](markdoc/members/types/IType.md#isnested)<br>Determines whether this type is nested | `get` |
| `string` | [`Name`](markdoc/members/types/IType.md#name)<br>Type name | `get` |
| `string` | [`RawName`](markdoc/members/types/IType.md#rawname)<br>Reflection fullname with namespace | `get` |
| `string` | [`TypeNamespace`](markdoc/members/types/IType.md#typenamespace)<br>Type namespace | `get` |

## Details
### Summary
Interface for types

### Properties
#### RawName
```csharp
public abstract string RawName { get }
```
##### Summary
Reflection fullname with namespace

#### Name
```csharp
public abstract string Name { get }
```
##### Summary
Type name

#### TypeNamespace
```csharp
public abstract string TypeNamespace { get }
```
##### Summary
Type namespace

#### IsNested
```csharp
public abstract bool IsNested { get }
```
##### Summary
Determines whether this type is nested

#### Accessor
```csharp
public abstract AccessorType Accessor { get }
```
##### Summary
Type accessor

*Generated with* [*MarkDoc*](https://github.com/hailstorm75/MarkDoc.Core)
