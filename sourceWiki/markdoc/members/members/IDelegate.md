# IDelegate `interface`

## Description
Interface for delegate members

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph MarkDoc.Members.Members
  MarkDoc.Members.Members.IDelegate[[IDelegate]]
  class MarkDoc.Members.Members.IDelegate interfaceStyle;
  MarkDoc.Members.Members.IMember[[IMember]]
  class MarkDoc.Members.Members.IMember interfaceStyle;
  end
MarkDoc.Members.Members.IMember --> MarkDoc.Members.Members.IDelegate
```

## Members
### Properties
#### Public  properties
| Type | Name | Methods |
| --- | --- | --- |
| `IReadOnlyCollection`&lt;[`IArgument`](./IArgument.md)&gt; | [`Arguments`](markdoc/members/members/IDelegate.md#arguments)<br>Delegate arguments | `get` |
| `IReadOnlyDictionary`&lt;`string`, `IReadOnlyCollection`&gt; | [`Generics`](markdoc/members/members/IDelegate.md#generics)<br>Method generic arguments | `get` |
| [`IResType`](../resolvedtypes/IResType.md) | [`Returns`](markdoc/members/members/IDelegate.md#returns)<br>Method return type | `get` |

## Details
### Summary
Interface for delegate members

### Inheritance
 - [
`IMember`
](./IMember.md)

### Properties
#### Arguments
```csharp
public abstract IReadOnlyCollection Arguments { get }
```
##### Summary
Delegate arguments

#### Generics
```csharp
public abstract IReadOnlyDictionary Generics { get }
```
##### Summary
Method generic arguments

#### Returns
```csharp
public abstract IResType Returns { get }
```
##### Summary
Method return type

*Generated with* [*MarkDoc*](https://github.com/hailstorm75/MarkDoc.Core)
