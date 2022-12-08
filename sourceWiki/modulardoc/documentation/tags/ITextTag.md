# ITextTag `interface`

## Description
Interface for tags containing text

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph ModularDoc.Documentation.Tags
  ModularDoc.Documentation.Tags.ITextTag[[ITextTag]]
  class ModularDoc.Documentation.Tags.ITextTag interfaceStyle;
  ModularDoc.Documentation.Tags.IContent[[IContent]]
  class ModularDoc.Documentation.Tags.IContent interfaceStyle;
  end
ModularDoc.Documentation.Tags.IContent --> ModularDoc.Documentation.Tags.ITextTag
```

## Members
### Properties
#### Public  properties
| Type | Name | Methods |
| --- | --- | --- |
| `string` | [`Content`](#content)<br>Text content | `get` |

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
public string Content { get; }
```
##### Summary
Text content

*Generated with* [*ModularDoc*](https://github.com/hailstorm75/ModularDoc)
