# ITextContent `interface`

## Description
Interface for elements representing test-based content

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph ModularDoc.Elements
  ModularDoc.Elements.ITextContent[[ITextContent]]
  class ModularDoc.Elements.ITextContent interfaceStyle;
  ModularDoc.Elements.IElement[[IElement]]
  class ModularDoc.Elements.IElement interfaceStyle;
  end
ModularDoc.Elements.IElement --> ModularDoc.Elements.ITextContent
```

## Details
### Summary
Interface for elements representing test-based content

### Inheritance
 - [
`IElement`
](./IElement.md)

*Generated with* [*ModularDoc*](https://github.com/hailstorm75/ModularDoc)
