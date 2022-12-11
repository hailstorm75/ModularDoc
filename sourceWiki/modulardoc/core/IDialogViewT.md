# IDialogView&lt;out TViewModel&gt; `interface`

## Description
Interface for dialog views

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph ModularDoc.Core
  ModularDoc.Core.IDialogViewModel[[IDialogViewModel]]
  class ModularDoc.Core.IDialogViewModel interfaceStyle;
  ModularDoc.Core.IDialogView_1[[IDialogView< TViewModel >]]
  class ModularDoc.Core.IDialogView_1 interfaceStyle;
  ModularDoc.Core.IDialogView_1TViewModel((TViewModel));
  ModularDoc.Core.IDialogView_1 -- where --o ModularDoc.Core.IDialogView_1TViewModel
ModularDoc.Core.IDialogViewModel --> ModularDoc.Core.IDialogView_1TViewModel

  ModularDoc.Core.IViewModel[[IViewModel]]
  class ModularDoc.Core.IViewModel interfaceStyle;
  ModularDoc.Core.IView_1[[IView< TViewModel >]]
  class ModularDoc.Core.IView_1 interfaceStyle;
  ModularDoc.Core.IView_1TViewModel((TViewModel));
  ModularDoc.Core.IView_1 -- where --o ModularDoc.Core.IView_1TViewModel
ModularDoc.Core.IViewModel --> ModularDoc.Core.IView_1TViewModel

  ModularDoc.Core.IView[[IView]]
  class ModularDoc.Core.IView interfaceStyle;
  ModularDoc.Core.IDialogView[[IDialogView]]
  class ModularDoc.Core.IDialogView interfaceStyle;
  ModularDoc.Core.IView[[IView]]
  class ModularDoc.Core.IView interfaceStyle;
  end
ModularDoc.Core.IView_1 --> ModularDoc.Core.IDialogView_1
ModularDoc.Core.IView --> ModularDoc.Core.IView_1
ModularDoc.Core.IDialogView --> ModularDoc.Core.IDialogView_1
ModularDoc.Core.IView --> ModularDoc.Core.IDialogView
```

## Details
### Summary
Interface for dialog views

### Generic types
| Type | Description | Constraints |
| --- | --- | --- |
| `TViewModel` `out` | View model type for dialog view | [`IDialogViewModel`](./IDialogViewModel.md) |

### Inheritance
 - [`IView`](./IViewT.md)&lt;`TViewModel`&gt;
 - [
`IView`
](./IView.md)
 - [
`IDialogView`
](./IDialogView.md)

*Generated with* [*ModularDoc*](https://github.com/hailstorm75/ModularDoc)
