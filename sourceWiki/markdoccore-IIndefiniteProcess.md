# IIndefiniteProcess `interface`

## Description
Interface for indefinite processes

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph MarkDoc.Core
  MarkDoc.Core.IIndefiniteProcess[[IIndefiniteProcess]]
  class MarkDoc.Core.IIndefiniteProcess interfaceStyle;
  MarkDoc.Core.IProcess[[IProcess]]
  class MarkDoc.Core.IProcess interfaceStyle;
  end
MarkDoc.Core.IProcess --> MarkDoc.Core.IIndefiniteProcess
```

## Details
### Summary
Interface for indefinite processes

### Inheritance
 - [
`IProcess`
](./markdoccore-IProcess.md)

*Generated with* [*MarkDoc*](https://github.com/hailstorm75/MarkDoc.Core)
