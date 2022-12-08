# IView&lt;out TViewModel&gt; `interface`

## Description
Interface for views

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph ModularDoc
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
ModularDoc.IView --> ModularDoc.IView_1
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
