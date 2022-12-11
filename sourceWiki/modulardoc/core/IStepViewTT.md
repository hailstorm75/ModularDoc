# IStepView&lt;out TViewModel,  TSettings&gt; `interface`

## Description
Interface for views of plugin steps

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph ModularDoc.Core
  ModularDoc.Core.ILibrarySettings[[ILibrarySettings]]
  class ModularDoc.Core.ILibrarySettings interfaceStyle;
  ModularDoc.Core.IStepViewModel_1[[IStepViewModel< TSettings >]]
  class ModularDoc.Core.IStepViewModel_1 interfaceStyle;
  ModularDoc.Core.IStepViewModel_1TSettings((TSettings));
  ModularDoc.Core.IStepViewModel_1 -- where --o ModularDoc.Core.IStepViewModel_1TSettings
ModularDoc.Core.ILibrarySettings --> ModularDoc.Core.IStepViewModel_1TSettings

  ModularDoc.Core.IStepView_2[[IStepView< TViewModel,TSettings >]]
  class ModularDoc.Core.IStepView_2 interfaceStyle;
  ModularDoc.Core.IStepView_2TViewModel((TViewModel));
  ModularDoc.Core.IStepView_2 -- where --o ModularDoc.Core.IStepView_2TViewModel
ModularDoc.Core.IStepViewModel_1 --> ModularDoc.Core.IStepView_2TViewModel
  ModularDoc.Core.IStepView_2TSettings((TSettings));
  ModularDoc.Core.IStepView_2 -- where --o ModularDoc.Core.IStepView_2TSettings
ModularDoc.Core.ILibrarySettings --> ModularDoc.Core.IStepView_2TSettings

  ModularDoc.Core.IStepViewModel[[IStepViewModel]]
  class ModularDoc.Core.IStepViewModel interfaceStyle;
  ModularDoc.Core.IStepView_1[[IStepView< TViewModel >]]
  class ModularDoc.Core.IStepView_1 interfaceStyle;
  ModularDoc.Core.IStepView_1TViewModel((TViewModel));
  ModularDoc.Core.IStepView_1 -- where --o ModularDoc.Core.IStepView_1TViewModel
ModularDoc.Core.IStepViewModel --> ModularDoc.Core.IStepView_1TViewModel

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
ModularDoc.Core.IStepView_1 --> ModularDoc.Core.IStepView_2
ModularDoc.Core.IView_1 --> ModularDoc.Core.IStepView_1
ModularDoc.Core.IView --> ModularDoc.Core.IView_1
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
