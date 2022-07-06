# IInnerTag `interface`

## Description
Interface for tags which are within other tags

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph MarkDoc.Documentation.Tags
  MarkDoc.Documentation.Tags.IInnerTag[[IInnerTag]]
  class MarkDoc.Documentation.Tags.IInnerTag interfaceStyle;
  MarkDoc.Documentation.Tags.IContent[[IContent]]
  class MarkDoc.Documentation.Tags.IContent interfaceStyle;
  end
MarkDoc.Documentation.Tags.IContent --> MarkDoc.Documentation.Tags.IInnerTag
```

## Members
### Properties
#### Public  properties
| Type | Name | Methods |
| --- | --- | --- |
| `IReadOnlyCollection`&lt;[`IContent`](./IContent.md)&gt; | [`Content`](#content)<br>Tag content | `get` |
| `string` | [`Reference`](#reference)<br>Tag reference | `get` |
| `InnerTagType` | [`Type`](#type)<br>Tag type | `get` |

## Details
### Summary
Interface for tags which are within other tags

### Inheritance
 - [
`IContent`
](./IContent.md)

### Nested types
#### Enums
 - `InnerTagType`

### Properties
#### Type
```csharp
public InnerTagType Type { get; }
```
##### Summary
Tag type

#### Reference
```csharp
public string Reference { get; }
```
##### Summary
Tag reference

##### Remarks
Either holds cref or name

#### Content
```csharp
public IReadOnlyCollection<IContent> Content { get; }
```
##### Summary
Tag content

*Generated with* [*MarkDoc*](https://github.com/hailstorm75/MarkDoc.Core)
