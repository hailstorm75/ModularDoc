# IHasHeading `interface`

## Description
Interface for types which have headings

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph ModularDoc.Elements.Extensions
  ModularDoc.Elements.Extensions.IHasHeading[[IHasHeading]]
  class ModularDoc.Elements.Extensions.IHasHeading interfaceStyle;
  end
```

## Members
### Properties
#### Public  properties
| Type | Name | Methods |
| --- | --- | --- |
| `string` | [`Heading`](#heading)<br>Heading text | `get` |
| `int` | [`Level`](#level)<br>Heading level | `get` |

## Details
### Summary
Interface for types which have headings

### Properties
#### Heading
```csharp
public string Heading { get; }
```
##### Summary
Heading text

#### Level
```csharp
public int Level { get; }
```
##### Summary
Heading level

*Generated with* [*ModularDoc*](https://github.com/hailstorm75/ModularDoc)
