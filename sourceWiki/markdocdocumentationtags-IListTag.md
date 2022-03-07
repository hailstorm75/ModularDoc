# IListTag `interface`

## Description
Interface for documentation list tags

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph MarkDoc.Documentation.Tags
  MarkDoc.Documentation.Tags.IListTag[[IListTag]]
  class MarkDoc.Documentation.Tags.IListTag interfaceStyle;
  MarkDoc.Documentation.Tags.IContent[[IContent]]
  class MarkDoc.Documentation.Tags.IContent interfaceStyle;
  end
MarkDoc.Documentation.Tags.IContent --> MarkDoc.Documentation.Tags.IListTag
```

## Members
### Properties
#### Public  properties
| Type | Name | Methods |
| --- | --- | --- |
| `IReadOnlyCollection`&lt;[`IContent`](./markdocdocumentationtags-IContent.md)&gt; | [`Headings`](markdocdocumentationtags-IListTag.md#headings)<br>List headings | `get` |
| `IReadOnlyCollection`&lt;`IReadOnlyCollection`&gt; | [`Rows`](markdocdocumentationtags-IListTag.md#rows)<br>List rows | `get` |
| `ListType` | [`Type`](markdocdocumentationtags-IListTag.md#type)<br>Type of list | `get` |

## Details
### Summary
Interface for documentation list tags

### Inheritance
 - [
`IContent`
](./markdocdocumentationtags-IContent.md)

### Nested types
#### Enums
 - `ListType`

### Properties
#### Type
```csharp
public abstract ListType Type { get }
```
##### Summary
Type of list

#### Headings
```csharp
public abstract IReadOnlyCollection Headings { get }
```
##### Summary
List headings

##### Remarks
Used when the list is a [ListType](./markdocdocumentationtagsilisttag-ListType.md).[Table](markdocdocumentationtags-IListTag.md#table)

#### Rows
```csharp
public abstract IReadOnlyCollection Rows { get }
```
##### Summary
List rows

*Generated with* [*MarkDoc*](https://github.com/hailstorm75/MarkDoc.Core)
