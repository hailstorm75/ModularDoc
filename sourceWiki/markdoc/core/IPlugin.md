# IPlugin `interface`

## Description
Interface for plugins

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph MarkDoc.Core
  MarkDoc.Core.IPlugin[[IPlugin]]
  class MarkDoc.Core.IPlugin interfaceStyle;
  end
```

## Members
### Properties
#### Public  properties
| Type | Name | Methods |
| --- | --- | --- |
| `string` | [`Author`](markdoc/core/IPlugin.md#author)<br>Plugin author | `get` |
| `string` | [`Description`](markdoc/core/IPlugin.md#description)<br>Plugin description | `get` |
| `string` | [`Id`](markdoc/core/IPlugin.md#id)<br>Plugin id | `get` |
| `Stream` | [`Image`](markdoc/core/IPlugin.md#image)<br>Plugin image | `get` |
| `string` | [`Name`](markdoc/core/IPlugin.md#name)<br>Plugin name | `get` |
| `IReadOnlyCollection`&lt;`string`&gt; | [`Steps`](markdoc/core/IPlugin.md#steps)<br>List of plugin step names | `get` |

### Methods
#### Public  methods
| Returns | Name |
| --- | --- |
| `(IMarkDocLogger logger, IReadOnlyCollection processes, Func executor)` | [`GenerateExecutor`](markdoc/core/IPlugin.md#generateexecutor)(`IReadOnlyDictionary`&lt;`string`, `IReadOnlyDictionary`&gt; configuration) |
| `IReadOnlyCollection` | [`GetPluginSteps`](markdoc/core/IPlugin.md#getpluginsteps)()<br>Get the [IPluginStep](./IPluginStep.md) instances |
| `T` | [`GetSettings`](markdoc/core/IPlugin.md#getsettings)(`IReadOnlyDictionary`&lt;`string`, `IReadOnlyDictionary`&gt; data) |

## Details
### Summary
Interface for plugins

### Methods
#### GetPluginSteps
```csharp
public abstract IReadOnlyCollection GetPluginSteps()
```
##### Summary
Get the [IPluginStep](./IPluginStep.md) instances

##### Returns


#### GetSettings
```csharp
public abstract T GetSettings<T>(IReadOnlyDictionary<string, IReadOnlyDictionary> data)
where T : ILibrarySettings
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `IReadOnlyDictionary`&lt;`string`, `IReadOnlyDictionary`&gt; | data |   |

#### GenerateExecutor
```csharp
public abstract (IMarkDocLogger logger, IReadOnlyCollection processes, Func executor) GenerateExecutor(IReadOnlyDictionary<string, IReadOnlyDictionary> configuration)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `IReadOnlyDictionary`&lt;`string`, `IReadOnlyDictionary`&gt; | configuration |   |

### Properties
#### Id
```csharp
public abstract string Id { get }
```
##### Summary
Plugin id

#### Name
```csharp
public abstract string Name { get }
```
##### Summary
Plugin name

#### Description
```csharp
public abstract string Description { get }
```
##### Summary
Plugin description

#### Author
```csharp
public abstract string Author { get }
```
##### Summary
Plugin author

#### Image
```csharp
public abstract Stream Image { get }
```
##### Summary
Plugin image

#### Steps
```csharp
public abstract IReadOnlyCollection Steps { get }
```
##### Summary
List of plugin step names

*Generated with* [*MarkDoc*](https://github.com/hailstorm75/MarkDoc.Core)
