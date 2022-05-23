# IProperty `interface`

## Description
Interface for properties

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph MarkDoc.Members.Members
  MarkDoc.Members.Members.IProperty[[IProperty]]
  class MarkDoc.Members.Members.IProperty interfaceStyle;
  MarkDoc.Members.Members.IMember[[IMember]]
  class MarkDoc.Members.Members.IMember interfaceStyle;
  end
MarkDoc.Members.Members.IMember --> MarkDoc.Members.Members.IProperty
```

## Members
### Properties
#### Public  properties
| Type | Name | Methods |
| --- | --- | --- |
| `Nullable`&lt;[`AccessorType`](../enums/AccessorType.md)&gt; | [`GetAccessor`](markdoc/members/members/IProperty.md#getaccessor)<br>Property get accessor type | `get` |
| [`MemberInheritance`](../enums/MemberInheritance.md) | [`Inheritance`](markdoc/members/members/IProperty.md#inheritance)<br>Property visibility | `get` |
| `bool` | [`IsReadOnly`](markdoc/members/members/IProperty.md#isreadonly)<br>Determines whether the property is readonly | `get` |
| `bool` | [`IsSetInit`](markdoc/members/members/IProperty.md#issetinit)<br>Is the property setter an init type | `get` |
| `Nullable`&lt;[`AccessorType`](../enums/AccessorType.md)&gt; | [`SetAccessor`](markdoc/members/members/IProperty.md#setaccessor)<br>Property set accessor type | `get` |
| [`IResType`](../resolvedtypes/IResType.md) | [`Type`](markdoc/members/members/IProperty.md#type)<br>Property type | `get` |

## Details
### Summary
Interface for properties

### Inheritance
 - [
`IMember`
](./IMember.md)

### Properties
#### Inheritance
```csharp
public abstract MemberInheritance Inheritance { get }
```
##### Summary
Property visibility

#### Type
```csharp
public abstract IResType Type { get }
```
##### Summary
Property type

#### IsReadOnly
```csharp
public abstract bool IsReadOnly { get }
```
##### Summary
Determines whether the property is readonly

##### Remarks
Applicable only for [IStruct](../types/IStruct.md) properties

#### GetAccessor
```csharp
public abstract Nullable GetAccessor { get }
```
##### Summary
Property get accessor type

#### SetAccessor
```csharp
public abstract Nullable SetAccessor { get }
```
##### Summary
Property set accessor type

#### IsSetInit
```csharp
public abstract bool IsSetInit { get }
```
##### Summary
Is the property setter an init type

*Generated with* [*MarkDoc*](https://github.com/hailstorm75/MarkDoc.Core)