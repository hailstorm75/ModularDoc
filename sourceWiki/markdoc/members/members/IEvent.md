# IEvent `interface`

## Description
Interface for events

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph MarkDoc.Members.Members
  MarkDoc.Members.Members.IEvent[[IEvent]]
  class MarkDoc.Members.Members.IEvent interfaceStyle;
  MarkDoc.Members.Members.IMember[[IMember]]
  class MarkDoc.Members.Members.IMember interfaceStyle;
  end
MarkDoc.Members.Members.IMember --> MarkDoc.Members.Members.IEvent
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
public abstract MemberInheritance Inheritance { get; }
```
##### Summary
Event inheritance type

#### Type
```csharp
public abstract IResType Type { get; }
```
##### Summary
Event type

*Generated with* [*MarkDoc*](https://github.com/hailstorm75/MarkDoc.Core)
