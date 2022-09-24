# IHasContent&lt;out T&gt; `interface`

## Description
Interface for elements which have content

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph MarkDoc.Elements.Extensions
  MarkDoc.Elements.Extensions.IHasContent_1[[IHasContent< T >]]
  class MarkDoc.Elements.Extensions.IHasContent_1 interfaceStyle;

  end
```

## Members
### Properties
#### Public  properties
| Type | Name | Methods |
| --- | --- | --- |
| `T` | [`Content`](#content)<br>Element content | `get` |

## Details
### Summary
Interface for elements which have content

### Generic types
| Type | Description | Constraints |
| --- | --- | --- |
| `T` `out` | Content type |  |

### Properties
#### Content
```csharp
public T Content { get; }
```
##### Summary
Element content

*Generated with* [*MarkDoc*](https://github.com/hailstorm75/MarkDoc.Core)
