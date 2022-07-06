# IDocumentation `interface`

## Description
Interface for documentation containers

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph MarkDoc.Documentation
  MarkDoc.Documentation.IDocumentation[[IDocumentation]]
  class MarkDoc.Documentation.IDocumentation interfaceStyle;
  end
```

## Members
### Properties
#### Public  properties
| Type | Name | Methods |
| --- | --- | --- |
| `bool` | [`HasInheritDoc`](#hasinheritdoc)<br>Contains the `inheritdoc` tag | `get` |
| `string` | [`InheritDocRef`](#inheritdocref)<br>InheritDoc reference | `get` |
| `IReadOnlyDictionary`&lt;`TagType`, `IReadOnlyCollection`&lt;[`ITag`](tags/ITag.md)&gt;&gt; | [`Tags`](#tags)<br>Documentation tags | `get` |

## Details
### Summary
Interface for documentation containers

### Properties
#### Tags
```csharp
public IReadOnlyDictionary<TagType, IReadOnlyCollection<ITag>> Tags { get; }
```
##### Summary
Documentation tags

#### HasInheritDoc
```csharp
public bool HasInheritDoc { get; }
```
##### Summary
Contains the `inheritdoc` tag

#### InheritDocRef
```csharp
public string InheritDocRef { get; }
```
##### Summary
InheritDoc reference

*Generated with* [*MarkDoc*](https://github.com/hailstorm75/MarkDoc.Core)
