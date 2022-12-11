# IView&lt;out TViewModel&gt; `interface`

## Description
Interface for views

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph ModularDoc.Core
  ModularDoc.Core.IViewModel[[IViewModel]]
  class ModularDoc.Core.IViewModel interfaceStyle;
  ModularDoc.Core.IView_1[[IView< TViewModel >]]
  class ModularDoc.Core.IView_1 interfaceStyle;
  ModularDoc.Core.IView_1TViewModel((TViewModel));
  ModularDoc.Core.IView_1 -- where --o ModularDoc.Core.IView_1TViewModel
ModularDoc.Core.IViewModel --> ModularDoc.Core.IView_1TViewModel

  ModularDoc.Core.IView[[IView]]
  class ModularDoc.Core.IView interfaceStyle;
  end
ModularDoc.Core.IView --> ModularDoc.Core.IView_1
```

## Members
### Properties
#### Public  properties
| Type | Name | Methods |
| --- | --- | --- |
| `TViewModel` | [`ViewModel`](#viewmodel)<br>View model | `get` |

## Details
### Summary
Interface for views

### Generic types
| Type | Description | Constraints |
| --- | --- | --- |
| `TViewModel` `out` | View model type for the view | [`IViewModel`](./IViewModel.md) |

### Inheritance
 - [
`IView`
](./IView.md)

### Properties
#### ViewModel
```csharp
public TViewModel ViewModel { get; }
```
##### Summary
View model

*Generated with* [*ModularDoc*](https://github.com/hailstorm75/ModularDoc)
