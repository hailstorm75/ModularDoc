# IRecord `interface`

## Description
Interface for records

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph ModularDoc.Members.Types
  ModularDoc.Members.Types.IRecord[[IRecord]]
  class ModularDoc.Members.Types.IRecord interfaceStyle;
  ModularDoc.Members.Types.IClass[[IClass]]
  class ModularDoc.Members.Types.IClass interfaceStyle;
  ModularDoc.Members.Types.IInterface[[IInterface]]
  class ModularDoc.Members.Types.IInterface interfaceStyle;
  ModularDoc.Members.Types.IType[[IType]]
  class ModularDoc.Members.Types.IType interfaceStyle;
  end
ModularDoc.Members.Types.IClass --> ModularDoc.Members.Types.IRecord
ModularDoc.Members.Types.IInterface --> ModularDoc.Members.Types.IClass
ModularDoc.Members.Types.IType --> ModularDoc.Members.Types.IInterface
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

*Generated with* [*ModularDoc*](https://github.com/hailstorm75/ModularDoc)
