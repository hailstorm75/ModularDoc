# IList `interface`

## Description
Interface for list elements

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph MarkDoc.Elements
  MarkDoc.Elements.IList[[IList]]
  class MarkDoc.Elements.IList interfaceStyle;
  MarkDoc.Elements.IElement[[IElement]]
  class MarkDoc.Elements.IElement interfaceStyle;
  end
  subgraph MarkDoc.Elements.Extensions
MarkDoc.Elements.Extensions.IHasContent_1<System.Collections.Generic.IReadOnlyCollection_1[[IHasContent]]
  MarkDoc.Elements.Extensions.IHasHeading[[IHasHeading]]
  class MarkDoc.Elements.Extensions.IHasHeading interfaceStyle;
  end
MarkDoc.Elements.IElement --> MarkDoc.Elements.IList
MarkDoc.Elements.Extensions.IHasContent_1<System.Collections.Generic.IReadOnlyCollection_1 --> MarkDoc.Elements.IList
MarkDoc.Elements.Extensions.IHasHeading --> MarkDoc.Elements.IList
```

## Members
### Properties
#### Public  properties
| Type | Name | Methods |
| --- | --- | --- |
| `ListType` | [`Type`](#type)<br>List type | `get` |

### Methods
#### Public  methods
| Returns | Name |
| --- | --- |
| `IEnumerable`&lt;`string`&gt; | [`Print`](#print)(`int` indent)<br>Prints element to a string |

## Details
### Summary
Interface for list elements

### Inheritance
 - [
`IElement`
](./IElement.md)
 - `IHasContent`&lt;`IReadOnlyCollection`&lt;[`IElement`](./IElement.md)&gt;&gt;
 - [
`IHasHeading`
](extensions/IHasHeading.md)

### Nested types
#### Enums
 - `ListType`

### Methods
#### Print
```csharp
public abstract IEnumerable<string> Print(int indent)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `int` | indent | List indentation level |

##### Summary
Prints element to a string

##### Returns
Converted list

### Properties
#### Type
```csharp
public abstract ListType Type { get; }
```
##### Summary
List type

*Generated with* [*MarkDoc*](https://github.com/hailstorm75/MarkDoc.Core)
