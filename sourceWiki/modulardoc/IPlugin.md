# IPlugin `interface`

## Description
Interface for plugins

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph ModularDoc
  ModularDoc.IPlugin[[IPlugin]]
  class ModularDoc.IPlugin interfaceStyle;
  end
```

## Members
### Properties
#### Public  properties
| Type | Name | Methods |
| --- | --- | --- |
| `string` | [`Author`](#author)<br>Plugin author | `get` |
| `string` | [`Description`](#description)<br>Plugin description | `get` |
| `string` | [`Id`](#id)<br>Plugin id | `get` |
| `Stream` | [`Image`](#image)<br>Plugin image | `get` |
| `string` | [`Name`](#name)<br>Plugin name | `get` |
| `IReadOnlyCollection`&lt;`string`&gt; | [`Steps`](#steps)<br>List of plugin step names | `get` |

### Methods
#### Public  methods
| Returns | Name |
| --- | --- |
| ([`IModularDocLogger`](./IModularDocLogger.md) logger, `IReadOnlyCollection`&lt;[`IProcess`](./IProcess.md)&gt; processes, `Func`&lt;`CancellationToken`, `ValueTask`&gt; executor) | [`GenerateExecutor`](#generateexecutor)(`IReadOnlyDictionary`&lt;`string`, `IReadOnlyDictionary`&lt;`string`, `string`&gt;&gt; configuration) |
| `IReadOnlyCollection`&lt;[`IPluginStep`](./IPluginStep.md)&gt; | [`GetPluginSteps`](#getpluginsteps)()<br>Get the [IPluginStep](./IPluginStep.md) instances |
| `T` | [`GetSettings`](#getsettings)(`IReadOnlyDictionary`&lt;`string`, `IReadOnlyDictionary`&lt;`string`, `string`&gt;&gt; data) |

## Details
### Summary
Interface for plugins

### Methods
#### GetPluginSteps
```csharp
public IReadOnlyCollection<IPluginStep> GetPluginSteps()
```
##### Summary
Get the [IPluginStep](./IPluginStep.md) instances

##### Returns


#### GetSettings
```csharp
public T GetSettings<T>(IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> data)
where T : ILibrarySettings
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `IReadOnlyDictionary`&lt;`string`, `IReadOnlyDictionary`&lt;`string`, `string`&gt;&gt; | data |   |

#### GenerateExecutor
```csharp
public (IModularDocLogger logger, IReadOnlyCollection<IProcess> processes, Func<CancellationToken, ValueTask> executor) GenerateExecutor(IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> configuration)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `IReadOnlyDictionary`&lt;`string`, `IReadOnlyDictionary`&lt;`string`, `string`&gt;&gt; | configuration |   |

### Properties
#### Id
```csharp
public string Id { get; }
```
##### Summary
Plugin id

#### Name
```csharp
public string Name { get; }
```
##### Summary
Plugin name

#### Description
```csharp
public string Description { get; }
```
##### Summary
Plugin description

#### Author
```csharp
public string Author { get; }
```
##### Summary
Plugin author

#### Image
```csharp
public Stream Image { get; }
```
##### Summary
Plugin image

#### Steps
```csharp
public IReadOnlyCollection<string> Steps { get; }
```
##### Summary
List of plugin step names

*Generated with* [*ModularDoc*](https://github.com/hailstorm75/ModularDoc)
