# IText `interface`

## Description
Interface for text elements

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph MarkDoc.Elements
  MarkDoc.Elements.IText[[IText]]
  class MarkDoc.Elements.IText interfaceStyle;
  MarkDoc.Elements.ITextContent[[ITextContent]]
  class MarkDoc.Elements.ITextContent interfaceStyle;
  MarkDoc.Elements.IElement[[IElement]]
  class MarkDoc.Elements.IElement interfaceStyle;
  end
  subgraph MarkDoc.Elements.Extensions
  MarkDoc.Elements.Extensions.IHasContent_1[[IHasContent< T >]]
  class MarkDoc.Elements.Extensions.IHasContent_1 interfaceStyle;

  end
MarkDoc.Elements.ITextContent --> MarkDoc.Elements.IText
MarkDoc.Elements.IElement --> MarkDoc.Elements.IText
MarkDoc.Elements.Extensions.IHasContent_1 --> MarkDoc.Elements.IText
```

## Members
### Properties
#### Public  properties
| Type | Name | Methods |
| --- | --- | --- |
| `TextStyle` | [`Style`](markdocelements-IText.md#style)<br>Text element style | `get` |

## Details
### Summary
Interface for text elements

### Inheritance
 - [
`ITextContent`
](./markdocelements-ITextContent.md)
 - [
`IElement`
](./markdocelements-IElement.md)
 - [`IHasContent`](./markdocelementsextensions-IHasContentT.md)&lt;`string`&gt;

### Nested types
#### Enums
 - `TextStyle`

### Properties
#### Style
```csharp
public abstract TextStyle Style { get }
```
##### Summary
Text element style

##### See also
 - 

*Generated with* [*MarkDoc*](https://github.com/hailstorm75/MarkDoc.Core)
