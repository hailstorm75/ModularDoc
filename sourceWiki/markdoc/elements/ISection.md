# ISection `interface`

## Description
Interface for section elements

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph MarkDoc.Elements
  MarkDoc.Elements.ISection[[ISection]]
  class MarkDoc.Elements.ISection interfaceStyle;
  MarkDoc.Elements.IElement[[IElement]]
  class MarkDoc.Elements.IElement interfaceStyle;
  end
  subgraph MarkDoc.Elements.Extensions
MarkDoc.Elements.Extensions.IHasContent_1<System.Collections.Generic.IReadOnlyCollection_1[[IHasContent]]
  MarkDoc.Elements.Extensions.IHasHeading[[IHasHeading]]
  class MarkDoc.Elements.Extensions.IHasHeading interfaceStyle;
  end
MarkDoc.Elements.IElement --> MarkDoc.Elements.ISection
MarkDoc.Elements.Extensions.IHasContent_1<System.Collections.Generic.IReadOnlyCollection_1 --> MarkDoc.Elements.ISection
MarkDoc.Elements.Extensions.IHasHeading --> MarkDoc.Elements.ISection
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

*Generated with* [*MarkDoc*](https://github.com/hailstorm75/MarkDoc.Core)
