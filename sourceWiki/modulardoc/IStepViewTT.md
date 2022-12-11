# IStepView&lt;out TViewModel,  TSettings&gt; `interface`

## Description
Interface for views of plugin steps

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph ModularDoc
  ModularDoc.ILibrarySettings[[ILibrarySettings]]
  class ModularDoc.ILibrarySettings interfaceStyle;
  ModularDoc.IStepViewModel_1[[IStepViewModel< TSettings >]]
  class ModularDoc.IStepViewModel_1 interfaceStyle;
  ModularDoc.IStepViewModel_1TSettings((TSettings));
  ModularDoc.IStepViewModel_1 -- where --o ModularDoc.IStepViewModel_1TSettings
ModularDoc.ILibrarySettings --> ModularDoc.IStepViewModel_1TSettings

  ModularDoc.IStepView_2[[IStepView< TViewModel,TSettings >]]
  class ModularDoc.IStepView_2 interfaceStyle;
  ModularDoc.IStepView_2TViewModel((TViewModel));
  ModularDoc.IStepView_2 -- where --o ModularDoc.IStepView_2TViewModel
ModularDoc.IStepViewModel_1 --> ModularDoc.IStepView_2TViewModel
  ModularDoc.IStepView_2TSettings((TSettings));
  ModularDoc.IStepView_2 -- where --o ModularDoc.IStepView_2TSettings
ModularDoc.ILibrarySettings --> ModularDoc.IStepView_2TSettings

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
ModularDoc.IStepView_1 --> ModularDoc.IStepView_2
ModularDoc.IView_1 --> ModularDoc.IStepView_1
ModularDoc.IView --> ModularDoc.IView_1
```

## Details
### Summary
Interface for views of plugin steps

### Generic types
| Type | Description | Constraints |
| --- | --- | --- |
| `TViewModel` `out` | View model type for the view | [`IStepViewModel`](./IStepViewModelT.md)&lt;`TSettings`&gt; |
| `TSettings` | Step settings type | [`ILibrarySettings`](./ILibrarySettings.md) |

### Inheritance
 - [`IStepView`](./IStepViewT.md)&lt;`TViewModel`&gt;
 - [`IView`](./IViewT.md)&lt;`TViewModel`&gt;
 - [
`IView`
](./IView.md)

*Generated with* [*ModularDoc*](https://github.com/hailstorm75/ModularDoc)