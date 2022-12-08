# IConstructor `interface`

## Description
Interface for type constructors

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph ModularDoc.Members.Members
  ModularDoc.Members.Members.IConstructor[[IConstructor]]
  class ModularDoc.Members.Members.IConstructor interfaceStyle;
  ModularDoc.Members.Members.IMember[[IMember]]
  class ModularDoc.Members.Members.IMember interfaceStyle;
  end
ModularDoc.Members.Members.IMember --> ModularDoc.Members.Members.IConstructor
```

## Members
### Properties
#### Public  properties
| Type | Name | Methods |
| --- | --- | --- |
| `IReadOnlyCollection`&lt;[`IArgument`](./IArgument.md)&gt; | [`Arguments`](#arguments)<br>Method arguments | `get` |

## Details
### Summary
Interface for type constructors

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
Method arguments

*Generated with* [*ModularDoc*](https://github.com/hailstorm75/ModularDoc)
