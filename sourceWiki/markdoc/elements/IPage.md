# IPage `interface`

## Description
Interface for page elements

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph MarkDoc.Elements
  MarkDoc.Elements.IPage[[IPage]]
  class MarkDoc.Elements.IPage interfaceStyle;
  MarkDoc.Elements.IElement[[IElement]]
  class MarkDoc.Elements.IElement interfaceStyle;
  end
  subgraph MarkDoc.Elements.Extensions
MarkDoc.Elements.Extensions.IHasContent_1<System.Collections.Generic.IReadOnlyCollection_1[[IHasContent]]
  MarkDoc.Elements.Extensions.IHasHeading[[IHasHeading]]
  class MarkDoc.Elements.Extensions.IHasHeading interfaceStyle;
  end
MarkDoc.Elements.IElement --> MarkDoc.Elements.IPage
MarkDoc.Elements.Extensions.IHasContent_1<System.Collections.Generic.IReadOnlyCollection_1 --> MarkDoc.Elements.IPage
MarkDoc.Elements.Extensions.IHasHeading --> MarkDoc.Elements.IPage
```

## Members
### Properties
#### Public  properties
| Type | Name | Methods |
| --- | --- | --- |
| `IReadOnlyCollection`&lt;[`IPage`](markdoc/elements/IPage.md)&gt; | [`Subpages`](markdoc/elements/IPage.md#subpages)<br>Pages within this given page | `get` |

## Details
### Summary
Interface for page elements

### Inheritance
 - [
`IElement`
](./IElement.md)
 - `IHasContent`&lt;`IReadOnlyCollection`&gt;
 - [
`IHasHeading`
](extensions/IHasHeading.md)

### Properties
#### Subpages
```csharp
public abstract IReadOnlyCollection Subpages { get }
```
##### Summary
Pages within this given page

##### Value
Collection of sub pages

*Generated with* [*MarkDoc*](https://github.com/hailstorm75/MarkDoc.Core)
