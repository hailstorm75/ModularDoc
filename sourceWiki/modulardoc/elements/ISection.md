# ISection `interface`

## Description
Interface for section elements

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph ModularDoc.Elements
  ModularDoc.Elements.ISection[[ISection]]
  class ModularDoc.Elements.ISection interfaceStyle;
  ModularDoc.Elements.IElement[[IElement]]
  class ModularDoc.Elements.IElement interfaceStyle;
  end
  subgraph ModularDoc.Elements.Extensions
ModularDoc.Elements.Extensions.IHasContent_1[[IHasContent]]
  ModularDoc.Elements.Extensions.IHasHeading[[IHasHeading]]
  class ModularDoc.Elements.Extensions.IHasHeading interfaceStyle;
  end
ModularDoc.Elements.IElement --> ModularDoc.Elements.ISection
ModularDoc.Elements.Extensions.IHasContent_1 --> ModularDoc.Elements.ISection
ModularDoc.Elements.Extensions.IHasHeading --> ModularDoc.Elements.ISection
```

## Details
### Summary
Interface for section elements

### Inheritance
 - [
`IElement`
](./IElement.md)
 - `IHasContent`&lt;`IReadOnlyCollection`&lt;[`IElement`](./IElement.md)&gt;&gt;
 - [
`IHasHeading`
](extensions/IHasHeading.md)

*Generated with* [*ModularDoc*](https://github.com/hailstorm75/ModularDoc)
