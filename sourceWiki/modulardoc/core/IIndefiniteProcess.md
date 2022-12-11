# IIndefiniteProcess `interface`

## Description
Interface for indefinite processes

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph ModularDoc.Core
  ModularDoc.Core.IIndefiniteProcess[[IIndefiniteProcess]]
  class ModularDoc.Core.IIndefiniteProcess interfaceStyle;
  ModularDoc.Core.IProcess[[IProcess]]
  class ModularDoc.Core.IProcess interfaceStyle;
  end
ModularDoc.Core.IProcess --> ModularDoc.Core.IIndefiniteProcess
```

## Details
### Summary
Interface for indefinite processes

### Inheritance
 - [
`IProcess`
](./IProcess.md)

*Generated with* [*ModularDoc*](https://github.com/hailstorm75/ModularDoc)
