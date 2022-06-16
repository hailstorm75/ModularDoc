# IProcess `interface`

## Description
Interface for processes

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph MarkDoc.Core
  MarkDoc.Core.IProcess[[IProcess]]
  class MarkDoc.Core.IProcess interfaceStyle;
  end
```

## Members
### Properties
#### Public  properties
| Type | Name | Methods |
| --- | --- | --- |
| `string` | [`Name`](#name)<br>Progress name | `get` |
| `ProcessState` | [`State`](#state)<br>State of the given process | `get, set` |

## Details
### Summary
Interface for processes

### Nested types
#### Enums
 - `ProcessState`

### Properties
#### Name
```csharp
public abstract string Name { get; }
```
##### Summary
Progress name

#### State
```csharp
public abstract ProcessState State { get; set; }
```
##### Summary
State of the given process

### Events
#### StateChanged
```csharp
public event EventHandler<ProcessState> StateChanged
```
##### Summary
Invoked whenever the [IProcess](markdoc/core/IProcess.md).[State](#state) is changed

*Generated with* [*MarkDoc*](https://github.com/hailstorm75/MarkDoc.Core)
