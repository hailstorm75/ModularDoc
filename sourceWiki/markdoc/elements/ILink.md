# ILink `interface`

## Description
Interface for link elements

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph MarkDoc.Elements
  MarkDoc.Elements.ILink[[ILink]]
  class MarkDoc.Elements.ILink interfaceStyle;
  MarkDoc.Elements.ITextContent[[ITextContent]]
  class MarkDoc.Elements.ITextContent interfaceStyle;
  MarkDoc.Elements.IElement[[IElement]]
  class MarkDoc.Elements.IElement interfaceStyle;
  end
  subgraph MarkDoc.Elements.Extensions
  MarkDoc.Elements.Extensions.IHasContent_1[[IHasContent< T >]]
  class MarkDoc.Elements.Extensions.IHasContent_1 interfaceStyle;

  end
MarkDoc.Elements.ITextContent --> MarkDoc.Elements.ILink
MarkDoc.Elements.IElement --> MarkDoc.Elements.ITextContent
MarkDoc.Elements.Extensions.IHasContent_1 --> MarkDoc.Elements.ILink
```

## Members
### Properties
#### Public  properties
| Type | Name | Methods |
| --- | --- | --- |
| `Lazy`&lt;`string`&gt; | [`Reference`](#reference)<br>Link reference | `get` |

## Details
### Summary
Interface for link elements

### Inheritance
 - [
`ITextContent`
](./ITextContent.md)
 - [
`IElement`
](./IElement.md)
 - [`IHasContent`](extensions/IHasContentT.md)&lt;[`IText`](./IText.md)&gt;

### Properties
#### Reference
```csharp
public Lazy<string> Reference { get; }
```
##### Summary
Link reference

##### Value
String containing a URI

*Generated with* [*MarkDoc*](https://github.com/hailstorm75/MarkDoc.Core)
