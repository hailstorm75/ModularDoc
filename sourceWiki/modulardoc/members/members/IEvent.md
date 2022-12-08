# IEvent `interface`

## Description
Interface for events

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph ModularDoc.Members.Members
  ModularDoc.Members.Members.IEvent[[IEvent]]
  class ModularDoc.Members.Members.IEvent interfaceStyle;
  ModularDoc.Members.Members.IMember[[IMember]]
  class ModularDoc.Members.Members.IMember interfaceStyle;
  end
ModularDoc.Members.Members.IMember --> ModularDoc.Members.Members.IEvent
```

## Members
### Properties
#### Public  properties
| Type | Name | Methods |
| --- | --- | --- |
| [`MemberInheritance`](../enums/MemberInheritance.md) | [`Inheritance`](#inheritance)<br>Event inheritance type | `get` |
| [`IResType`](../resolvedtypes/IResType.md) | [`Type`](#type)<br>Event type | `get` |

## Details
### Summary
Interface for events

### Inheritance
 - [
`IMember`
](./IMember.md)

### Properties
#### Inheritance
```csharp
public MemberInheritance Inheritance { get; }
```
##### Summary
Event inheritance type

#### Type
```csharp
public IResType Type { get; }
```
##### Summary
Event type

*Generated with* [*ModularDoc*](https://github.com/hailstorm75/ModularDoc)
