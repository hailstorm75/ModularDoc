# IStepView&lt;out TViewModel&gt; `interface`

## Description
Interface for views of plugin steps

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph ModularDoc
  ModularDoc.IStepViewModel[[IStepViewModel]]
  class ModularDoc.IStepViewModel interfaceStyle;
  ModularDoc.IStepView_1[[IStepView< TViewModel >]]
  class ModularDoc.IStepView_1 interfaceStyle;
  ModularDoc.IStepView_1TViewModel((TViewModel));
  ModularDoc.IStepView_1 -- where --o ModularDoc.IStepView_1TViewModel
ModularDoc.IStepViewModel --> ModularDoc.IStepView_1TViewModel

  ModularDoc.IViewModel[[IViewModel]]
  class ModularDoc.IViewModel interfaceStyle;
  ModularDoc.IView_1[[IView< TViewModel >]]
  class ModularDoc.IView_1 interfaceStyle;
  ModularDoc.IView_1TViewModel((TViewModel));
  ModularDoc.IView_1 -- where --o ModularDoc.IView_1TViewModel
ModularDoc.IViewModel --> ModularDoc.IView_1TViewModel

  ModularDoc.IView[[IView]]
  class ModularDoc.IView interfaceStyle;
  end
ModularDoc.IView_1 --> ModularDoc.IStepView_1
ModularDoc.IView --> ModularDoc.IView_1
```

## Members
### Properties
#### Public  properties
| Type | Name | Methods |
| --- | --- | --- |
| `string` | [`Id`](#id)<br>Step view Id | `get` |

### Methods
#### Public  methods
| Returns | Name |
| --- | --- |
| `Task` | [`SetPreviousSettingsAsync`](#setprevioussettingsasync)(`IReadOnlyDictionary`&lt;`string`, `IReadOnlyDictionary`&lt;`string`, `string`&gt;&gt; settings) |

## Details
### Summary
Interface for views of plugin steps

### Generic types
| Type | Description | Constraints |
| --- | --- | --- |
| `TViewModel` `out` | View model type for the view | [`IStepViewModel`](./IStepViewModel.md) |

### Inheritance
 - [`IView`](./IViewT.md)&lt;`TViewModel`&gt;
 - [
`IView`
](./IView.md)

### Methods
#### SetPreviousSettingsAsync
```csharp
public async Task SetPreviousSettingsAsync(IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> settings)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `IReadOnlyDictionary`&lt;`string`, `IReadOnlyDictionary`&lt;`string`, `string`&gt;&gt; | settings |   |

### Properties
#### Id
```csharp
public string Id { get; }
```
##### Summary
Step view Id

*Generated with* [*ModularDoc*](https://github.com/hailstorm75/ModularDoc)
