# IGlobalSettings `interface`

## Description
Interface for global settings

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph MarkDoc.Core
  MarkDoc.Core.IGlobalSettings[[IGlobalSettings]]
  class MarkDoc.Core.IGlobalSettings interfaceStyle;
  MarkDoc.Core.ILibrarySettings[[ILibrarySettings]]
  class MarkDoc.Core.ILibrarySettings interfaceStyle;
  end
MarkDoc.Core.ILibrarySettings --> MarkDoc.Core.IGlobalSettings
```

## Members
### Properties
#### Public  properties
| Type | Name | Methods |
| --- | --- | --- |
| `IReadOnlyCollection`&lt;`string`&gt; | [`CheckedIgnoredNamespaces`](markdoccore-IGlobalSettings.md#checkedignorednamespaces)<br>Globally ignored but previously selected namespaces | `get` |
| `IReadOnlyCollection`&lt;`string`&gt; | [`CheckedIgnoredTypes`](markdoccore-IGlobalSettings.md#checkedignoredtypes)<br>Globally ignored but previously selected types | `get` |
| `IReadOnlyCollection`&lt;`string`&gt; | [`IgnoredNamespaces`](markdoccore-IGlobalSettings.md#ignorednamespaces)<br>Globally ignored namespaces | `get` |
| `IReadOnlyCollection`&lt;`string`&gt; | [`IgnoredTypes`](markdoccore-IGlobalSettings.md#ignoredtypes)<br>Globally ignored types | `get` |
| `string` | [`OutputPath`](markdoccore-IGlobalSettings.md#outputpath)<br>Output directory | `get` |

## Details
### Summary
Interface for global settings

### Inheritance
 - [
`ILibrarySettings`
](./markdoccore-ILibrarySettings.md)

### Properties
#### IgnoredNamespaces
```csharp
public abstract IReadOnlyCollection IgnoredNamespaces { get }
```
##### Summary
Globally ignored namespaces

#### IgnoredTypes
```csharp
public abstract IReadOnlyCollection IgnoredTypes { get }
```
##### Summary
Globally ignored types

#### CheckedIgnoredNamespaces
```csharp
public abstract IReadOnlyCollection CheckedIgnoredNamespaces { get }
```
##### Summary
Globally ignored but previously selected namespaces

#### CheckedIgnoredTypes
```csharp
public abstract IReadOnlyCollection CheckedIgnoredTypes { get }
```
##### Summary
Globally ignored but previously selected types

#### OutputPath
```csharp
public abstract string OutputPath { get }
```
##### Summary
Output directory

*Generated with* [*MarkDoc*](https://github.com/hailstorm75/MarkDoc.Core)
