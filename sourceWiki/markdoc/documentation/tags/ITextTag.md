# ITextTag `interface`

## Description
Interface for tags containing text

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph MarkDoc.Documentation.Tags
  MarkDoc.Documentation.Tags.ITextTag[[ITextTag]]
  class MarkDoc.Documentation.Tags.ITextTag interfaceStyle;
  MarkDoc.Documentation.Tags.IContent[[IContent]]
  class MarkDoc.Documentation.Tags.IContent interfaceStyle;
  end
MarkDoc.Documentation.Tags.IContent --> MarkDoc.Documentation.Tags.ITextTag
```

## Members
### Properties
#### Public  properties
| Type | Name | Methods |
| --- | --- | --- |
| `string` | [`Content`](markdoc/documentation/tags/ITextTag.md#content)<br>Text content | `get` |

## Details
### Summary
Interface for tags containing text

### Inheritance
 - [
`IContent`
](./IContent.md)

### Properties
#### Content
```csharp
public abstract string Content { get; }
```
##### Summary
Text content

*Generated with* [*MarkDoc*](https://github.com/hailstorm75/MarkDoc.Core)
