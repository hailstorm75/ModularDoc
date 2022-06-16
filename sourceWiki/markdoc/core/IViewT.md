# IView&lt;out TViewModel&gt; `interface`

## Description
Interface for views

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph MarkDoc.Core
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
MarkDoc.Core.IView --> MarkDoc.Core.IView_1
```

## Members
### Properties
#### Public  properties
| Type | Name | Methods |
| --- | --- | --- |
| `TViewModel` | [`ViewModel`](markdoc/core/IViewT.md#viewmodel)<br>View model | `get` |

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
public abstract TViewModel ViewModel { get; }
```
##### Summary
View model

*Generated with* [*MarkDoc*](https://github.com/hailstorm75/MarkDoc.Core)
