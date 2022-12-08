# ITable `interface`

## Description
Interface for the table element

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph ModularDoc.Elements
  ModularDoc.Elements.ITable[[ITable]]
  class ModularDoc.Elements.ITable interfaceStyle;
  ModularDoc.Elements.IElement[[IElement]]
  class ModularDoc.Elements.IElement interfaceStyle;
  end
  subgraph ModularDoc.Elements.Extensions
ModularDoc.Elements.Extensions.IHasContent_1[[IHasContent]]
  ModularDoc.Elements.Extensions.IHasHeading[[IHasHeading]]
  class ModularDoc.Elements.Extensions.IHasHeading interfaceStyle;
  end
ModularDoc.Elements.IElement --> ModularDoc.Elements.ITable
ModularDoc.Elements.Extensions.IHasContent_1 --> ModularDoc.Elements.ITable
ModularDoc.Elements.Extensions.IHasHeading --> ModularDoc.Elements.ITable
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

*Generated with* [*ModularDoc*](https://github.com/hailstorm75/ModularDoc)
