# IDialogView&lt;out TViewModel&gt; `interface`

## Description
Interface for dialog views

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph ModularDoc
  ModularDoc.IDialogViewModel[[IDialogViewModel]]
  class ModularDoc.IDialogViewModel interfaceStyle;
  ModularDoc.IDialogView_1[[IDialogView< TViewModel >]]
  class ModularDoc.IDialogView_1 interfaceStyle;
  ModularDoc.IDialogView_1TViewModel((TViewModel));
  ModularDoc.IDialogView_1 -- where --o ModularDoc.IDialogView_1TViewModel
ModularDoc.IDialogViewModel --> ModularDoc.IDialogView_1TViewModel

  ModularDoc.IViewModel[[IViewModel]]
  class ModularDoc.IViewModel interfaceStyle;
  ModularDoc.IView_1[[IView< TViewModel >]]
  class ModularDoc.IView_1 interfaceStyle;
  ModularDoc.IView_1TViewModel((TViewModel));
  ModularDoc.IView_1 -- where --o ModularDoc.IView_1TViewModel
ModularDoc.IViewModel --> ModularDoc.IView_1TViewModel

  ModularDoc.IView[[IView]]
  class ModularDoc.IView interfaceStyle;
  ModularDoc.IDialogView[[IDialogView]]
  class ModularDoc.IDialogView interfaceStyle;
  ModularDoc.IView[[IView]]
  class ModularDoc.IView interfaceStyle;
  end
ModularDoc.IView_1 --> ModularDoc.IDialogView_1
ModularDoc.IView --> ModularDoc.IView_1
ModularDoc.IDialogView --> ModularDoc.IDialogView_1
ModularDoc.IView --> ModularDoc.IDialogView
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
