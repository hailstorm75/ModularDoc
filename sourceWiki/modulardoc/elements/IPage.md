# IPage `interface`

## Description
Interface for page elements

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph ModularDoc.Elements
  ModularDoc.Elements.IPage[[IPage]]
  class ModularDoc.Elements.IPage interfaceStyle;
  ModularDoc.Elements.IElement[[IElement]]
  class ModularDoc.Elements.IElement interfaceStyle;
  end
  subgraph ModularDoc.Elements.Extensions
ModularDoc.Elements.Extensions.IHasContent_1[[IHasContent]]
  ModularDoc.Elements.Extensions.IHasHeading[[IHasHeading]]
  class ModularDoc.Elements.Extensions.IHasHeading interfaceStyle;
  end
ModularDoc.Elements.IElement --> ModularDoc.Elements.IPage
ModularDoc.Elements.Extensions.IHasContent_1 --> ModularDoc.Elements.IPage
ModularDoc.Elements.Extensions.IHasHeading --> ModularDoc.Elements.IPage
```

## Members
### Properties
#### Public  properties
| Type | Name | Methods |
| --- | --- | --- |
| `IReadOnlyCollection`&lt;[`IPage`](modulardoc/elements/IPage.md)&gt; | [`Subpages`](#subpages)<br>Pages within this given page | `get` |

## Details
### Summary
Interface for page elements

### Inheritance
 - [
`IElement`
](./IElement.md)
 - `IHasContent`&lt;`IReadOnlyCollection`&lt;[`IElement`](./IElement.md)&gt;&gt;
 - [
`IHasHeading`
](extensions/IHasHeading.md)

### Properties
#### Subpages
```csharp
public IReadOnlyCollection<IPage> Subpages { get; }
```
##### Summary
Pages within this given page

##### Value
Collection of sub pages

*Generated with* [*ModularDoc*](https://github.com/hailstorm75/ModularDoc)
