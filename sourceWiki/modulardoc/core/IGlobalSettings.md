# IGlobalSettings `interface`

## Description
Interface for global settings

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph ModularDoc.Core
  ModularDoc.Core.IGlobalSettings[[IGlobalSettings]]
  class ModularDoc.Core.IGlobalSettings interfaceStyle;
  ModularDoc.Core.ILibrarySettings[[ILibrarySettings]]
  class ModularDoc.Core.ILibrarySettings interfaceStyle;
  end
ModularDoc.Core.ILibrarySettings --> ModularDoc.Core.IGlobalSettings
```

## Members
### Properties
#### Public  properties
| Type | Name | Methods |
| --- | --- | --- |
| `IReadOnlyCollection`&lt;`string`&gt; | [`CheckedIgnoredNamespaces`](#checkedignorednamespaces)<br>Globally ignored but previously selected namespaces | `get` |
| `IReadOnlyCollection`&lt;`string`&gt; | [`CheckedIgnoredTypes`](#checkedignoredtypes)<br>Globally ignored but previously selected types | `get` |
| `IReadOnlyCollection`&lt;`string`&gt; | [`IgnoredNamespaces`](#ignorednamespaces)<br>Globally ignored namespaces | `get` |
| `IReadOnlyCollection`&lt;`string`&gt; | [`IgnoredTypes`](#ignoredtypes)<br>Globally ignored types | `get` |
| `string` | [`OutputPath`](#outputpath)<br>Output directory | `get` |

## Details
### Summary
Interface for global settings

### Inheritance
 - [
`ILibrarySettings`
](./ILibrarySettings.md)

### Properties
#### IgnoredNamespaces
```csharp
public IReadOnlyCollection<string> IgnoredNamespaces { get; }
```
##### Summary
Globally ignored namespaces

#### IgnoredTypes
```csharp
public IReadOnlyCollection<string> IgnoredTypes { get; }
```
##### Summary
Globally ignored types

#### CheckedIgnoredNamespaces
```csharp
public IReadOnlyCollection<string> CheckedIgnoredNamespaces { get; }
```
##### Summary
Globally ignored but previously selected namespaces

#### CheckedIgnoredTypes
```csharp
public IReadOnlyCollection<string> CheckedIgnoredTypes { get; }
```
##### Summary
Globally ignored but previously selected types

#### OutputPath
```csharp
public string OutputPath { get; }
```
##### Summary
Output directory

*Generated with* [*ModularDoc*](https://github.com/hailstorm75/ModularDoc)
