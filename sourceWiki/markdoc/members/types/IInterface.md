# IInterface `interface`

## Description
Interface for interface types

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph MarkDoc.Members.Types
  MarkDoc.Members.Types.IInterface[[IInterface]]
  class MarkDoc.Members.Types.IInterface interfaceStyle;
  MarkDoc.Members.Types.IType[[IType]]
  class MarkDoc.Members.Types.IType interfaceStyle;
  end
MarkDoc.Members.Types.IType --> MarkDoc.Members.Types.IInterface
```

## Members
### Properties
#### Public  properties
| Type | Name | Methods |
| --- | --- | --- |
| `IReadOnlyCollection`&lt;[`IDelegate`](../members/IDelegate.md)&gt; | [`Delegates`](markdoc/members/types/IInterface.md#delegates)<br>Collection of delegates | `get` |
| `IReadOnlyCollection`&lt;[`IEvent`](../members/IEvent.md)&gt; | [`Events`](markdoc/members/types/IInterface.md#events)<br>Collection of events | `get` |
| `IReadOnlyDictionary`&lt;`string`, `(Variance Item1, IReadOnlyCollection Item2)`&gt; | [`Generics`](markdoc/members/types/IInterface.md#generics)<br>Generics name, and their variance and constraints | `get` |
| `Lazy`&lt;`IReadOnlyDictionary`&lt;[`IMember`](../members/IMember.md), [`IInterface`](markdoc/members/types/IInterface.md)&gt;&gt; | [`InheritedTypeMembers`](markdoc/members/types/IInterface.md#inheritedtypemembers)<br>Members inherited from derived types | `get` |
| `IReadOnlyCollection`&lt;[`IResType`](../resolvedtypes/IResType.md)&gt; | [`InheritedTypesFlat`](markdoc/members/types/IInterface.md#inheritedtypesflat)<br>Collection of inherited types | `get` |
| `Lazy`&lt;`IReadOnlyCollection`&lt;[`TreeNode`](./TreeNode.md)&gt;&gt; | [`InheritedTypesStructured`](markdoc/members/types/IInterface.md#inheritedtypesstructured)<br>Structured inherited types | `get` |
| `IReadOnlyCollection`&lt;[`IMethod`](../members/IMethod.md)&gt; | [`Methods`](markdoc/members/types/IInterface.md#methods)<br>Collection of methods | `get` |
| `IReadOnlyCollection`&lt;[`IType`](./IType.md)&gt; | [`NestedTypes`](markdoc/members/types/IInterface.md#nestedtypes)<br>Collection of nested types | `get` |
| `IReadOnlyCollection`&lt;[`IProperty`](../members/IProperty.md)&gt; | [`Properties`](markdoc/members/types/IInterface.md#properties)<br>Collection of properties | `get` |

## Details
### Summary
Interface for interface types

### Inheritance
 - [
`IType`
](./IType.md)

### Properties
#### InheritedTypesFlat
```csharp
public abstract IReadOnlyCollection InheritedTypesFlat { get }
```
##### Summary
Collection of inherited types

#### InheritedTypesStructured
```csharp
public abstract Lazy InheritedTypesStructured { get }
```
##### Summary
Structured inherited types

#### Generics
```csharp
public abstract IReadOnlyDictionary Generics { get }
```
##### Summary
Generics name, and their variance and constraints

#### Delegates
```csharp
public abstract IReadOnlyCollection Delegates { get }
```
##### Summary
Collection of delegates

#### NestedTypes
```csharp
public abstract IReadOnlyCollection NestedTypes { get }
```
##### Summary
Collection of nested types

#### Events
```csharp
public abstract IReadOnlyCollection Events { get }
```
##### Summary
Collection of events

#### Methods
```csharp
public abstract IReadOnlyCollection Methods { get }
```
##### Summary
Collection of methods

#### Properties
```csharp
public abstract IReadOnlyCollection Properties { get }
```
##### Summary
Collection of properties

#### InheritedTypeMembers
```csharp
public abstract Lazy InheritedTypeMembers { get }
```
##### Summary
Members inherited from derived types

*Generated with* [*MarkDoc*](https://github.com/hailstorm75/MarkDoc.Core)
