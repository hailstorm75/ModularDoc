# ITextBuilder `interface`

## Description
Interface for joining multiple [ITextContent](./ITextContent.md) elements

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph ModularDoc.Elements
  ModularDoc.Elements.ITextBuilder[[ITextBuilder]]
  class ModularDoc.Elements.ITextBuilder interfaceStyle;
  ModularDoc.Elements.ITextContent[[ITextContent]]
  class ModularDoc.Elements.ITextContent interfaceStyle;
  ModularDoc.Elements.IElement[[IElement]]
  class ModularDoc.Elements.IElement interfaceStyle;
  end
  subgraph ModularDoc.Elements.Extensions
ModularDoc.Elements.Extensions.IHasContent_1[[IHasContent]]
  end
ModularDoc.Elements.ITextContent --> ModularDoc.Elements.ITextBuilder
ModularDoc.Elements.IElement --> ModularDoc.Elements.ITextContent
ModularDoc.Elements.Extensions.IHasContent_1 --> ModularDoc.Elements.ITextBuilder
```

## Members
### Properties
#### Public  properties
| Type | Name | Methods |
| --- | --- | --- |
| `string` | [`Delimiter`](#delimiter)<br>Joined text delimiter | `get` |

## Details
### Summary
Interface for joining multiple [ITextContent](./ITextContent.md) elements

### Inheritance
 - [
`ITextContent`
](./ITextContent.md)
 - [
`IElement`
](./IElement.md)
 - `IHasContent`&lt;`IReadOnlyCollection`&lt;[`ITextContent`](./ITextContent.md)&gt;&gt;

### Properties
#### Delimiter
```csharp
public string Delimiter { get; }
```
##### Summary
Joined text delimiter

*Generated with* [*ModularDoc*](https://github.com/hailstorm75/ModularDoc)
