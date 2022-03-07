# IStepView&lt;out TViewModel,  TSettings&gt; `interface`

## Description
Interface for views of plugin steps

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph MarkDoc.Core
  MarkDoc.Core.ILibrarySettings[[ILibrarySettings]]
  class MarkDoc.Core.ILibrarySettings interfaceStyle;
  MarkDoc.Core.IStepViewModel_1[[IStepViewModel< TSettings >]]
  class MarkDoc.Core.IStepViewModel_1 interfaceStyle;
  MarkDoc.Core.IStepViewModel_1TSettings((TSettings));
  MarkDoc.Core.IStepViewModel_1 -- where --o MarkDoc.Core.IStepViewModel_1TSettings
MarkDoc.Core.ILibrarySettings --> MarkDoc.Core.IStepViewModel_1TSettings

  MarkDoc.Core.IStepView_2[[IStepView< TViewModel,TSettings >]]
  class MarkDoc.Core.IStepView_2 interfaceStyle;
  MarkDoc.Core.IStepView_2TViewModel((TViewModel));
  MarkDoc.Core.IStepView_2 -- where --o MarkDoc.Core.IStepView_2TViewModel
MarkDoc.Core.IStepViewModel_1 --> MarkDoc.Core.IStepView_2TViewModel
  MarkDoc.Core.IStepView_2TSettings((TSettings));
  MarkDoc.Core.IStepView_2 -- where --o MarkDoc.Core.IStepView_2TSettings
MarkDoc.Core.ILibrarySettings --> MarkDoc.Core.IStepView_2TSettings

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
MarkDoc.Core.IStepView_1 --> MarkDoc.Core.IStepView_2
MarkDoc.Core.IView_1 --> MarkDoc.Core.IStepView_2
MarkDoc.Core.IView --> MarkDoc.Core.IStepView_2
```

## Details
### Summary
Interface for views of plugin steps

### Generic types
| Type | Description | Constraints |
| --- | --- | --- |
| `TViewModel` `out` | View model type for the view | [`IStepViewModel`](./markdoccore-IStepViewModelT.md)&lt;`TSettings`&gt; |
| `TSettings` | Step settings type | [`ILibrarySettings`](./markdoccore-ILibrarySettings.md) |

### Inheritance
 - [`IStepView`](./markdoccore-IStepViewT.md)&lt;`TViewModel`&gt;
 - [`IView`](./markdoccore-IViewT.md)&lt;`TViewModel`&gt;
 - [
`IView`
](./markdoccore-IView.md)

*Generated with* [*MarkDoc*](https://github.com/hailstorm75/MarkDoc.Core)
