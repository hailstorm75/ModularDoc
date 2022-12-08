# ProcessState `enum`

## Description
Enumeration of possible states of a [IProcess](../IProcess.md)

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph ModularDoc.IProcess
  ModularDoc.IProcess.ProcessState[[ProcessState]]
  end
```

## Details
### Summary
Enumeration of possible states of a [IProcess](../IProcess.md)

### Fields
#### Idle
##### Summary
The process is waiting

#### Running
##### Summary
The process is running

#### Success
##### Summary
The process has finished successfully

#### Failure
##### Summary
The process has finished unsuccessfully

#### Cancelled
##### Summary
The process has been cancelled

*Generated with* [*ModularDoc*](https://github.com/hailstorm75/ModularDoc)
