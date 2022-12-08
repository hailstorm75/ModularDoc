# ILink `interface`

## Description
Interface for link elements

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph ModularDoc.Elements
  ModularDoc.Elements.ILink[[ILink]]
  class ModularDoc.Elements.ILink interfaceStyle;
  ModularDoc.Elements.ITextContent[[ITextContent]]
  class ModularDoc.Elements.ITextContent interfaceStyle;
  ModularDoc.Elements.IElement[[IElement]]
  class ModularDoc.Elements.IElement interfaceStyle;
  end
  subgraph ModularDoc.Elements.Extensions
  ModularDoc.Elements.Extensions.IHasContent_1[[IHasContent< T >]]
  class ModularDoc.Elements.Extensions.IHasContent_1 interfaceStyle;

  end
ModularDoc.Elements.ITextContent --> ModularDoc.Elements.ILink
ModularDoc.Elements.IElement --> ModularDoc.Elements.ITextContent
ModularDoc.Elements.Extensions.IHasContent_1 --> ModularDoc.Elements.ILink
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

*Generated with* [*ModularDoc*](https://github.com/hailstorm75/ModularDoc)
