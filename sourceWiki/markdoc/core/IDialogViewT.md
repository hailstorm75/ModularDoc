# IDialogView&lt;out TViewModel&gt; `interface`

## Description
Interface for dialog views

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph MarkDoc.Core
  MarkDoc.Core.IDialogViewModel[[IDialogViewModel]]
  class MarkDoc.Core.IDialogViewModel interfaceStyle;
  MarkDoc.Core.IDialogView_1[[IDialogView< TViewModel >]]
  class MarkDoc.Core.IDialogView_1 interfaceStyle;
  MarkDoc.Core.IDialogView_1TViewModel((TViewModel));
  MarkDoc.Core.IDialogView_1 -- where --o MarkDoc.Core.IDialogView_1TViewModel
MarkDoc.Core.IDialogViewModel --> MarkDoc.Core.IDialogView_1TViewModel

  MarkDoc.Core.IViewModel[[IViewModel]]
  class MarkDoc.Core.IViewModel interfaceStyle;
  MarkDoc.Core.IView_1[[IView< TViewModel >]]
  class MarkDoc.Core.IView_1 interfaceStyle;
  MarkDoc.Core.IView_1TViewModel((TViewModel));
  MarkDoc.Core.IView_1 -- where --o MarkDoc.Core.IView_1TViewModel
MarkDoc.Core.IViewModel --> MarkDoc.Core.IView_1TViewModel

  MarkDoc.Core.IView[[IView]]
  class MarkDoc.Core.IView interfaceStyle;
  MarkDoc.Core.IDialogView[[IDialogView]]
  class MarkDoc.Core.IDialogView interfaceStyle;
  end
MarkDoc.Core.IView_1 --> MarkDoc.Core.IDialogView_1
MarkDoc.Core.IView --> MarkDoc.Core.IView_1
MarkDoc.Core.IDialogView --> MarkDoc.Core.IDialogView_1
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

*Generated with* [*MarkDoc*](https://github.com/hailstorm75/MarkDoc.Core)
