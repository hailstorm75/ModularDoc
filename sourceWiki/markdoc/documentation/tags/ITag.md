# ITag `interface`

## Description
Interface for documentation tags

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph MarkDoc.Documentation.Tags
  MarkDoc.Documentation.Tags.ITag[[ITag]]
  class MarkDoc.Documentation.Tags.ITag interfaceStyle;
  end
```

## Members
### Properties
#### Public  properties
| Type | Name | Methods |
| --- | --- | --- |
| `IReadOnlyCollection`&lt;[`IContent`](./IContent.md)&gt; | [`Content`](markdoc/documentation/tags/ITag.md#content)<br>Tag inner content | `get` |
| `string` | [`Reference`](markdoc/documentation/tags/ITag.md#reference)<br>Tag reference | `get` |
| `TagType` | [`Type`](markdoc/documentation/tags/ITag.md#type)<br>Tag type | `get` |

## Details
### Summary
Interface for documentation tags

### Nested types
#### Enums
 - `TagType`

### Properties
#### Type
```csharp
public abstract TagType Type { get; }
```
##### Summary
Tag type

##### See also
 - 

#### Reference
```csharp
public abstract string Reference { get; }
```
##### Summary
Tag reference

##### Remarks
Either holds cref or name

#### Content
```csharp
public abstract IReadOnlyCollection<IContent> Content { get; }
```
##### Summary
Tag inner content

*Generated with* [*MarkDoc*](https://github.com/hailstorm75/MarkDoc.Core)
