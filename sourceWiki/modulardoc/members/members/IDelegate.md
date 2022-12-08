# IDelegate `interface`

## Description
Interface for delegate types

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph ModularDoc.Members.Members
  ModularDoc.Members.Members.IDelegate[[IDelegate]]
  class ModularDoc.Members.Members.IDelegate interfaceStyle;
  ModularDoc.Members.Members.IMember[[IMember]]
  class ModularDoc.Members.Members.IMember interfaceStyle;
  end
ModularDoc.Members.Members.IMember --> ModularDoc.Members.Members.IDelegate
```

## Members
### Properties
#### Public  properties
| Type | Name | Methods |
| --- | --- | --- |
| `IReadOnlyCollection`&lt;[`IArgument`](./IArgument.md)&gt; | [`Arguments`](#arguments)<br>Delegate arguments | `get` |
| `IReadOnlyDictionary`&lt;`string`, `IReadOnlyCollection`&lt;[`IResType`](../resolvedtypes/IResType.md)&gt;&gt; | [`Generics`](#generics)<br>Method generic arguments | `get` |
| [`IResType`](../resolvedtypes/IResType.md) | [`Returns`](#returns)<br>Method return type | `get` |

## Details
### Summary
Interface for delegate types

### Inheritance
 - [
`IMember`
](./IMember.md)

### Properties
#### Arguments
```csharp
public IReadOnlyCollection<IArgument> Arguments { get; }
```
##### Summary
Delegate arguments

#### Generics
```csharp
public IReadOnlyDictionary<string, IReadOnlyCollection<IResType>> Generics { get; }
```
##### Summary
Method generic arguments

#### Returns
```csharp
public IResType Returns { get; }
```
##### Summary
Method return type

*Generated with* [*ModularDoc*](https://github.com/hailstorm75/ModularDoc)
