# IStepView&lt;out TViewModel&gt; `interface`

## Description
Interface for views of plugin steps

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph MarkDoc.Core
  MarkDoc.Core.IStepViewModel[[IStepViewModel]]
  class MarkDoc.Core.IStepViewModel interfaceStyle;
  MarkDoc.Core.IStepView_1[[IStepView< TViewModel >]]
  class MarkDoc.Core.IStepView_1 interfaceStyle;
  MarkDoc.Core.IStepView_1TViewModel((TViewModel));
  MarkDoc.Core.IStepView_1 -- where --o MarkDoc.Core.IStepView_1TViewModel
MarkDoc.Core.IStepViewModel --> MarkDoc.Core.IStepView_1TViewModel

  MarkDoc.Core.IViewModel[[IViewModel]]
  class MarkDoc.Core.IViewModel interfaceStyle;
  MarkDoc.Core.IView_1[[IView< TViewModel >]]
  class MarkDoc.Core.IView_1 interfaceStyle;
  MarkDoc.Core.IView_1TViewModel((TViewModel));
  MarkDoc.Core.IView_1 -- where --o MarkDoc.Core.IView_1TViewModel
MarkDoc.Core.IViewModel --> MarkDoc.Core.IView_1TViewModel

  MarkDoc.Core.IView[[IView]]
  class MarkDoc.Core.IView interfaceStyle;
  end
MarkDoc.Core.IView_1 --> MarkDoc.Core.IStepView_1
MarkDoc.Core.IView --> MarkDoc.Core.IView_1
```

## Members
### Properties
#### Public  properties
| Type | Name | Methods |
| --- | --- | --- |
| `string` | [`Id`](markdoc/core/IStepViewT.md#id)<br>Step view Id | `get` |

### Methods
#### Public  methods
| Returns | Name |
| --- | --- |
| `Task` | [`SetPreviousSettingsAsync`](markdoc/core/IStepViewT.md#setprevioussettingsasync)(`IReadOnlyDictionary`&lt;`string`, `IReadOnlyDictionary`&lt;`string`, `string`&gt;&gt; settings) |

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
public virtual async Task SetPreviousSettingsAsync(IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> settings)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `IReadOnlyDictionary`&lt;`string`, `IReadOnlyDictionary`&lt;`string`, `string`&gt;&gt; | settings |   |

### Properties
#### Id
```csharp
public virtual string Id { get }
```
##### Summary
Step view Id

*Generated with* [*MarkDoc*](https://github.com/hailstorm75/MarkDoc.Core)
