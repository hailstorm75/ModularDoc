# IElementCreator `interface`

## Description
Interface for [IElement](./IElement.md) creators

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph MarkDoc.Elements
  MarkDoc.Elements.IElementCreator[[IElementCreator]]
  class MarkDoc.Elements.IElementCreator interfaceStyle;
  end
```

## Members
### Methods
#### Public  methods
| Returns | Name |
| --- | --- |
| [`IDiagram`](./IDiagram.md) | [`CreateDiagram`](markdoc/elements/IElementCreator.md#creatediagram)(`string` name, `string` content)<br>Creates a new [IDiagram](./IDiagram.md) instance |
| [`ILink`](./ILink.md) | [`CreateLink`](markdoc/elements/IElementCreator.md#createlink)([`IText`](./IText.md) content, `Lazy`&lt;`string`&gt; reference) |
| [`IList`](./IList.md) | [`CreateList`](markdoc/elements/IElementCreator.md#createlist)(`IEnumerable`&lt;[`IElement`](./IElement.md)&gt; elements, `ListType` type, `string` heading, `int` level) |
| [`IPage`](./IPage.md) | [`CreatePage`](markdoc/elements/IElementCreator.md#createpage)(`IEnumerable`&lt;[`IPage`](./IPage.md)&gt; subpages, `IEnumerable`&lt;[`IElement`](./IElement.md)&gt; content, `string` heading, `int` level) |
| [`ISection`](./ISection.md) | [`CreateSection`](markdoc/elements/IElementCreator.md#createsection)(`IEnumerable`&lt;[`IElement`](./IElement.md)&gt; content, `string` heading, `int` level) |
| [`ITable`](./ITable.md) | [`CreateTable`](markdoc/elements/IElementCreator.md#createtable)(`IEnumerable`&lt;`IReadOnlyCollection`&lt;[`IElement`](./IElement.md)&gt;&gt; content, `IEnumerable`&lt;[`IText`](./IText.md)&gt; headings, `string` heading, `int` level) |
| [`IText`](./IText.md) | [`CreateText`](markdoc/elements/IElementCreator.md#createtext)(`string` content, `TextStyle` style) |
| [`ITextContent`](./ITextContent.md) | [`JoinTextContent`](markdoc/elements/IElementCreator.md#jointextcontent)(`IEnumerable`&lt;[`ITextContent`](./ITextContent.md)&gt; content, `string` delimiter) |

## Details
### Summary
Interface for [IElement](./IElement.md) creators

### Methods
#### CreateList
```csharp
public abstract IList CreateList(IEnumerable<IElement> elements, ListType type, string heading, int level)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `IEnumerable`&lt;[`IElement`](./IElement.md)&gt; | elements |   |
| `ListType` | type |   |
| `string` | heading |   |
| `int` | level |   |

#### CreateSection
```csharp
public abstract ISection CreateSection(IEnumerable<IElement> content, string heading, int level)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `IEnumerable`&lt;[`IElement`](./IElement.md)&gt; | content |   |
| `string` | heading |   |
| `int` | level |   |

#### CreateTable
```csharp
public abstract ITable CreateTable(IEnumerable<IReadOnlyCollection<IElement>> content, IEnumerable<IText> headings, string heading, int level)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `IEnumerable`&lt;`IReadOnlyCollection`&lt;[`IElement`](./IElement.md)&gt;&gt; | content |   |
| `IEnumerable`&lt;[`IText`](./IText.md)&gt; | headings |   |
| `string` | heading |   |
| `int` | level |   |

#### CreatePage
```csharp
public abstract IPage CreatePage(IEnumerable<IPage> subpages, IEnumerable<IElement> content, string heading, int level)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `IEnumerable`&lt;[`IPage`](./IPage.md)&gt; | subpages |   |
| `IEnumerable`&lt;[`IElement`](./IElement.md)&gt; | content |   |
| `string` | heading |   |
| `int` | level |   |

#### CreateText
```csharp
public abstract IText CreateText(string content, TextStyle style)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `string` | content |   |
| `TextStyle` | style |   |

#### CreateDiagram
```csharp
public abstract IDiagram CreateDiagram(string name, string content)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `string` | name | Diagram name |
| `string` | content | Diagram data source |

##### Summary
Creates a new [IDiagram](./IDiagram.md) instance

##### Returns


#### CreateLink
```csharp
public abstract ILink CreateLink(IText content, Lazy<string> reference)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| [`IText`](./IText.md) | content |   |
| `Lazy`&lt;`string`&gt; | reference |   |

#### JoinTextContent
```csharp
public abstract ITextContent JoinTextContent(IEnumerable<ITextContent> content, string delimiter)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `IEnumerable`&lt;[`ITextContent`](./ITextContent.md)&gt; | content |   |
| `string` | delimiter |   |

*Generated with* [*MarkDoc*](https://github.com/hailstorm75/MarkDoc.Core)
