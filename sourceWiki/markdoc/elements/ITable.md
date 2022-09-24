# ITable `interface`

## Description
Interface for the table element

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph MarkDoc.Elements
  MarkDoc.Elements.ITable[[ITable]]
  class MarkDoc.Elements.ITable interfaceStyle;
  MarkDoc.Elements.IElement[[IElement]]
  class MarkDoc.Elements.IElement interfaceStyle;
  end
  subgraph MarkDoc.Elements.Extensions
MarkDoc.Elements.Extensions.IHasContent_1[[IHasContent]]
  MarkDoc.Elements.Extensions.IHasHeading[[IHasHeading]]
  class MarkDoc.Elements.Extensions.IHasHeading interfaceStyle;
  end
MarkDoc.Elements.IElement --> MarkDoc.Elements.ITable
MarkDoc.Elements.Extensions.IHasContent_1 --> MarkDoc.Elements.ITable
MarkDoc.Elements.Extensions.IHasHeading --> MarkDoc.Elements.ITable
```

## Members
### Properties
#### Public  properties
| Type | Name | Methods |
| --- | --- | --- |
| `IReadOnlyCollection`&lt;[`IText`](./IText.md)&gt; | [`Headings`](#headings)<br>Table headers | `get` |

## Details
### Summary
Interface for the table element

### Inheritance
 - [
`IElement`
](./IElement.md)
 - `IHasContent`&lt;`IReadOnlyCollection`&lt;`IReadOnlyCollection`&lt;[`IElement`](./IElement.md)&gt;&gt;&gt;
 - [
`IHasHeading`
](extensions/IHasHeading.md)

### Properties
#### Headings
```csharp
public IReadOnlyCollection<IText> Headings { get; }
```
##### Summary
Table headers

##### Value
Collection of header names

*Generated with* [*MarkDoc*](https://github.com/hailstorm75/MarkDoc.Core)
