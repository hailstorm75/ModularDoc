# LogMessage `readonly struct`

## Description
Logger message structure

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph MarkDoc.Core
  MarkDoc.Core.LogMessage[[LogMessage]]
  class MarkDoc.Core.LogMessage interfaceStyle;
  end
```

## Members
### Properties
#### Public  properties
| Type | Name | Methods |
| --- | --- | --- |
| `string` | [`Message`](markdoc/core/LogMessage.md#message)<br>Log message | `get` |
| `DateTime` | [`Time`](markdoc/core/LogMessage.md#time)<br>Log time | `get` |
| `LogType` | [`Type`](markdoc/core/LogMessage.md#type)<br>Log type | `get` |

## Details
### Summary
Logger message structure

### Properties
#### Type
```csharp
public LogType Type { get }
```
##### Summary
Log type

#### Message
```csharp
public string Message { get }
```
##### Summary
Log message

#### Time
```csharp
public DateTime Time { get }
```
##### Summary
Log time

*Generated with* [*MarkDoc*](https://github.com/hailstorm75/MarkDoc.Core)
