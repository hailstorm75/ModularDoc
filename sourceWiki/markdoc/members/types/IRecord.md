# IRecord `interface`

## Description
Interface for records

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph MarkDoc.Members.Types
  MarkDoc.Members.Types.IRecord[[IRecord]]
  class MarkDoc.Members.Types.IRecord interfaceStyle;
  MarkDoc.Members.Types.IClass[[IClass]]
  class MarkDoc.Members.Types.IClass interfaceStyle;
  MarkDoc.Members.Types.IInterface[[IInterface]]
  class MarkDoc.Members.Types.IInterface interfaceStyle;
  MarkDoc.Members.Types.IType[[IType]]
  class MarkDoc.Members.Types.IType interfaceStyle;
  end
MarkDoc.Members.Types.IClass --> MarkDoc.Members.Types.IRecord
MarkDoc.Members.Types.IInterface --> MarkDoc.Members.Types.IClass
MarkDoc.Members.Types.IType --> MarkDoc.Members.Types.IInterface
```

## Details
### Summary
Interface for records

### Inheritance
 - [
`IClass`
](./IClass.md)
 - [
`IInterface`
](./IInterface.md)
 - [
`IType`
](./IType.md)

*Generated with* [*MarkDoc*](https://github.com/hailstorm75/MarkDoc.Core)
