# IMethod `interface`

## Description
Interface for methods

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph MarkDoc.Members.Members
  MarkDoc.Members.Members.IMethod[[IMethod]]
  class MarkDoc.Members.Members.IMethod interfaceStyle;
  MarkDoc.Members.Members.IConstructor[[IConstructor]]
  class MarkDoc.Members.Members.IConstructor interfaceStyle;
  MarkDoc.Members.Members.IMember[[IMember]]
  class MarkDoc.Members.Members.IMember interfaceStyle;
  end
MarkDoc.Members.Members.IConstructor --> MarkDoc.Members.Members.IMethod
MarkDoc.Members.Members.IMember --> MarkDoc.Members.Members.IConstructor
```

## Members
### Properties
#### Public  properties
| Type | Name | Methods |
| --- | --- | --- |
| `IReadOnlyDictionary`&lt;`string`, `IReadOnlyCollection`&lt;[`IResType`](../resolvedtypes/IResType.md)&gt;&gt; | [`Generics`](markdoc/members/members/IMethod.md#generics)<br>Method generic arguments | `get` |
| [`MemberInheritance`](../enums/MemberInheritance.md) | [`Inheritance`](markdoc/members/members/IMethod.md#inheritance)<br>Method visibility | `get` |
| `bool` | [`IsAsync`](markdoc/members/members/IMethod.md#isasync)<br>Determines whether the method is asynchronous | `get` |
| [`OperatorType`](../enums/OperatorType.md) | [`Operator`](markdoc/members/members/IMethod.md#operator)<br>Operator type | `get` |
| [`IResType`](../resolvedtypes/IResType.md) | [`Returns`](markdoc/members/members/IMethod.md#returns)<br>Method return type | `get` |

## Details
### Summary
Interface for methods

### Inheritance
 - [
`IConstructor`
](./IConstructor.md)
 - [
`IMember`
](./IMember.md)

### Properties
#### Inheritance
```csharp
public abstract MemberInheritance Inheritance { get; }
```
##### Summary
Method visibility

#### Generics
```csharp
public abstract IReadOnlyDictionary<string, IReadOnlyCollection<IResType>> Generics { get; }
```
##### Summary
Method generic arguments

#### IsAsync
```csharp
public abstract bool IsAsync { get; }
```
##### Summary
Determines whether the method is asynchronous

#### Operator
```csharp
public abstract OperatorType Operator { get; }
```
##### Summary
Operator type

#### Returns
```csharp
public abstract IResType Returns { get; }
```
##### Summary
Method return type

*Generated with* [*MarkDoc*](https://github.com/hailstorm75/MarkDoc.Core)
