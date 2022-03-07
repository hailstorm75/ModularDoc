# ITextBuilder `interface`

## Description
Interface for joining multiple [ITextContent](./markdocelements-ITextContent) elements

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph MarkDoc.Elements
  MarkDoc.Elements.ITextBuilder[[ITextBuilder]]
  class MarkDoc.Elements.ITextBuilder interfaceStyle;
  MarkDoc.Elements.ITextContent[[ITextContent]]
  class MarkDoc.Elements.ITextContent interfaceStyle;
  MarkDoc.Elements.IElement[[IElement]]
  class MarkDoc.Elements.IElement interfaceStyle;
  end
  subgraph MarkDoc.Elements.Extensions
MarkDoc.Elements.Extensions.IHasContent_1<System.Collections.Generic.IReadOnlyCollection_1[[IHasContent]]
  end
MarkDoc.Elements.ITextContent --> MarkDoc.Elements.ITextBuilder
MarkDoc.Elements.IElement --> MarkDoc.Elements.ITextBuilder
MarkDoc.Elements.Extensions.IHasContent_1<System.Collections.Generic.IReadOnlyCollection_1 --> MarkDoc.Elements.ITextBuilder
```

## Members
### Properties
#### Public  properties
| Type | Name | Methods |
| --- | --- | --- |
| `string` | [`Delimiter`](markdocelements-ITextBuilder#delimiter)<br>Joined text delimiter | `get` |

## Details
### Summary
Interface for joining multiple [ITextContent](./markdocelements-ITextContent) elements

### Inheritance
 - [
`ITextContent`
](./markdocelements-ITextContent)
 - [
`IElement`
](./markdocelements-IElement)
 - `IHasContent`&lt;`IReadOnlyCollection`&gt;

### Properties
#### Delimiter
```csharp
public abstract string Delimiter { get }
```
##### Summary
Joined text delimiter

*Generated with* [*MarkDoc*](https://github.com/hailstorm75/MarkDoc.Core)
