# IMarkDocLogger `interface`

## Description
Interface for in-house logger

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph MarkDoc.Core
  MarkDoc.Core.IMarkDocLogger[[IMarkDocLogger]]
  class MarkDoc.Core.IMarkDocLogger interfaceStyle;
  end
```

## Members
### Methods
#### Public  methods
| Returns | Name |
| --- | --- |
| `void` | [`Debug`](#debug)(`string` message)<br>Logs given `message` as debug information |
| `void` | [`Error`](#error)(`string` message)<br>Logs given `message` as an error |
| `void` | [`Info`](#info)(`string` message)<br>Logs given `message` as information |
| `void` | [`Warning`](#warning)(`string` message)<br>Logs given `message` as a warning |

## Details
### Summary
Interface for in-house logger

### Nested types
#### Enums
 - `LogType`

### Methods
#### Info
```csharp
public abstract void Info(string message)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `string` | message | Message to log |

##### Summary
Logs given `message` as information

#### Debug
```csharp
public abstract void Debug(string message)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `string` | message | Message to log |

##### Summary
Logs given `message` as debug information

#### Error
```csharp
public abstract void Error(string message)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `string` | message | Message to log |

##### Summary
Logs given `message` as an error

#### Warning
[*Source code*](https://github.com///blob//src/Libraries/Core/MarkDoc.Members/Types/TreeNode.cs#L14)
```csharp
public abstract void Warning(string message)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `string` | message | Message to log |

##### Summary
Logs given `message` as a warning

### Events
#### NewLog
```csharp
public event EventHandler<LogMessage> NewLog
```
##### Summary
Invoked when a new log is created

*Generated with* [*MarkDoc*](https://github.com/hailstorm75/MarkDoc.Core)
