# IView `interface`

## Description
Interface for views

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph MarkDoc.Core
  MarkDoc.Core.IView[[IView]]
  class MarkDoc.Core.IView interfaceStyle;
  end
```

## Members
### Methods
#### Public  methods
| Returns | Name |
| --- | --- |
| [`IViewModel`](./IViewModel.md) | [`GetViewModel`](#getviewmodel)()<br>Retrieves the views view models |
| `Task` | [`SetArguments`](#setarguments)(`IEnumerable`&lt;`string`&gt; arguments) |
| `Task` | [`SetNamedArgumentsAsync`](#setnamedargumentsasync)(`IReadOnlyDictionary`&lt;`string`, `string`&gt; arguments) |

## Details
### Summary
Interface for views

### Methods
#### GetViewModel
```csharp
public abstract IViewModel GetViewModel()
```
##### Summary
Retrieves the views view models

##### Returns
View model instance

#### SetArguments
```csharp
public virtual async Task SetArguments(IEnumerable<string> arguments)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `IEnumerable`&lt;`string`&gt; | arguments |   |

#### SetNamedArgumentsAsync
```csharp
public abstract Task SetNamedArgumentsAsync(IReadOnlyDictionary<string, string> arguments)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `IReadOnlyDictionary`&lt;`string`, `string`&gt; | arguments |   |

*Generated with* [*MarkDoc*](https://github.com/hailstorm75/MarkDoc.Core)
