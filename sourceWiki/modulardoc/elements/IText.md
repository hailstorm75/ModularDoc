# IText `interface`

## Description
Interface for text elements

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph ModularDoc.Elements
  ModularDoc.Elements.IText[[IText]]
  class ModularDoc.Elements.IText interfaceStyle;
  ModularDoc.Elements.ITextContent[[ITextContent]]
  class ModularDoc.Elements.ITextContent interfaceStyle;
  ModularDoc.Elements.IElement[[IElement]]
  class ModularDoc.Elements.IElement interfaceStyle;
  end
  subgraph ModularDoc.Elements.Extensions
  ModularDoc.Elements.Extensions.IHasContent_1[[IHasContent< T >]]
  class ModularDoc.Elements.Extensions.IHasContent_1 interfaceStyle;

  end
ModularDoc.Elements.ITextContent --> ModularDoc.Elements.IText
ModularDoc.Elements.IElement --> ModularDoc.Elements.ITextContent
ModularDoc.Elements.Extensions.IHasContent_1 --> ModularDoc.Elements.IText
```

## Members
### Properties
#### Public  properties
| Type | Name | Methods |
| --- | --- | --- |
| `TextStyle` | [`Style`](#style)<br>Text element style | `get` |

## Details
### Summary
Interface for text elements

### Inheritance
 - [
`ITextContent`
](./ITextContent.md)
 - [
`IElement`
](./IElement.md)
 - [`IHasContent`](extensions/IHasContentT.md)&lt;`string`&gt;

### Nested types
#### Enums
 - `TextStyle`

### Properties
#### Style
```csharp
public TextStyle Style { get; }
```
##### Summary
Text element style

##### See also
 - 

*Generated with* [*ModularDoc*](https://github.com/hailstorm75/ModularDoc)
