# IDefiniteProcess `interface`

## Description
Interface for definite processes

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph ModularDoc.Core
  ModularDoc.Core.IDefiniteProcess[[IDefiniteProcess]]
  class ModularDoc.Core.IDefiniteProcess interfaceStyle;
  ModularDoc.Core.IProcess[[IProcess]]
  class ModularDoc.Core.IProcess interfaceStyle;
  end
ModularDoc.Core.IProcess --> ModularDoc.Core.IDefiniteProcess
```

## Members
### Properties
#### Public  properties
| Type | Name | Methods |
| --- | --- | --- |
| `double` | [`Complete`](#complete)<br>Percentage complete | `get` |
| `int` | [`Current`](#current)<br>Completed parts so far | `get` |
| `int` | [`Max`](#max)<br>Number of parts to be completed | `get` |

### Methods
#### Public  methods
| Returns | Name |
| --- | --- |
| `void` | [`IncreaseCompletion`](#increasecompletion)()<br>Increases the number of complete parts |

## Details
### Summary
Interface for definite processes

### Inheritance
 - [
`IProcess`
](./IProcess.md)

### Methods
#### IncreaseCompletion
```csharp
public void IncreaseCompletion()
```
##### Summary
Increases the number of complete parts

### Properties
#### Complete
```csharp
public double Complete { get; }
```
##### Summary
Percentage complete

#### Current
```csharp
public int Current { get; }
```
##### Summary
Completed parts so far

#### Max
```csharp
public int Max { get; }
```
##### Summary
Number of parts to be completed

*Generated with* [*ModularDoc*](https://github.com/hailstorm75/ModularDoc)
