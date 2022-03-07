# IElementCreator `interface`

## Description
Interface for [IElement](./markdocelements-IElement) creators

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
| [`IDiagram`](./markdocelements-IDiagram) | [`CreateDiagram`](markdocelements-IElementCreator#creatediagram)(`string` name, `string` content)<br>Creates a new [IDiagram](./markdocelements-IDiagram) instance |
| [`ILink`](./markdocelements-ILink) | [`CreateLink`](markdocelements-IElementCreator#createlink)([`IText`](./markdocelements-IText) content, `Lazy`&lt;`string`&gt; reference) |
| [`IList`](./markdocelements-IList) | [`CreateList`](markdocelements-IElementCreator#createlist)(`IEnumerable`&lt;[`IElement`](./markdocelements-IElement)&gt; elements, `ListType` type, `string` heading, `int` level) |
| [`IPage`](./markdocelements-IPage) | [`CreatePage`](markdocelements-IElementCreator#createpage)(`IEnumerable`&lt;[`IPage`](./markdocelements-IPage)&gt; subpages, `IEnumerable`&lt;[`IElement`](./markdocelements-IElement)&gt; content, `string` heading, `int` level) |
| [`ISection`](./markdocelements-ISection) | [`CreateSection`](markdocelements-IElementCreator#createsection)(`IEnumerable`&lt;[`IElement`](./markdocelements-IElement)&gt; content, `string` heading, `int` level) |
| [`ITable`](./markdocelements-ITable) | [`CreateTable`](markdocelements-IElementCreator#createtable)(`IEnumerable`&lt;`IReadOnlyCollection`&gt; content, `IEnumerable`&lt;[`IText`](./markdocelements-IText)&gt; headings, `string` heading, `int` level) |
| [`IText`](./markdocelements-IText) | [`CreateText`](markdocelements-IElementCreator#createtext)(`string` content, `TextStyle` style) |
| [`ITextContent`](./markdocelements-ITextContent) | [`JoinTextContent`](markdocelements-IElementCreator#jointextcontent)(`IEnumerable`&lt;[`ITextContent`](./markdocelements-ITextContent)&gt; content, `string` delimiter) |

## Details
### Summary
Interface for [IElement](./markdocelements-IElement) creators

### Methods
#### CreateList
```csharp
public abstract IList CreateList(IEnumerable<IElement> elements, ListType type, string heading, int level)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `IEnumerable`&lt;[`IElement`](./markdocelements-IElement)&gt; | elements |   |
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
| `IEnumerable`&lt;[`IElement`](./markdocelements-IElement)&gt; | content |   |
| `string` | heading |   |
| `int` | level |   |

#### CreateTable
```csharp
public abstract ITable CreateTable(IEnumerable<IReadOnlyCollection> content, IEnumerable<IText> headings, string heading, int level)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `IEnumerable`&lt;`IReadOnlyCollection`&gt; | content |   |
| `IEnumerable`&lt;[`IText`](./markdocelements-IText)&gt; | headings |   |
| `string` | heading |   |
| `int` | level |   |

#### CreatePage
```csharp
public abstract IPage CreatePage(IEnumerable<IPage> subpages, IEnumerable<IElement> content, string heading, int level)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `IEnumerable`&lt;[`IPage`](./markdocelements-IPage)&gt; | subpages |   |
| `IEnumerable`&lt;[`IElement`](./markdocelements-IElement)&gt; | content |   |
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
Creates a new [IDiagram](./markdocelements-IDiagram) instance

##### Returns


#### CreateLink
```csharp
public abstract ILink CreateLink(IText content, Lazy<string> reference)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| [`IText`](./markdocelements-IText) | content |   |
| `Lazy`&lt;`string`&gt; | reference |   |

#### JoinTextContent
```csharp
public abstract ITextContent JoinTextContent(IEnumerable<ITextContent> content, string delimiter)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `IEnumerable`&lt;[`ITextContent`](./markdocelements-ITextContent)&gt; | content |   |
| `string` | delimiter |   |

*Generated with* [*MarkDoc*](https://github.com/hailstorm75/MarkDoc.Core)
