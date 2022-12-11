# IStepViewModel&lt; TSettings&gt; `interface`

## Description
Interface for view models of plugin steps

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

  ModularDoc.Core.IStepViewModel[[IStepViewModel]]
  class ModularDoc.Core.IStepViewModel interfaceStyle;
  ModularDoc.Core.IViewModel[[IViewModel]]
  class ModularDoc.Core.IViewModel interfaceStyle;
  end
  subgraph System
System.IDisposable[[IDisposable]]
  end
ModularDoc.Core.IStepViewModel --> ModularDoc.Core.IStepViewModel_1
ModularDoc.Core.IViewModel --> ModularDoc.Core.IStepViewModel
System.IDisposable --> ModularDoc.Core.IViewModel
```

## Details
### Summary
Interface for view models of plugin steps

### Generic types
| Type | Description | Constraints |
| --- | --- | --- |
| `TSettings` | Step settings type | [`ILibrarySettings`](./ILibrarySettings.md) |

### Inheritance
 - [
`IStepViewModel`
](./IStepViewModel.md)
 - [
`IViewModel`
](./IViewModel.md)
 - `IDisposable`

*Generated with* [*ModularDoc*](https://github.com/hailstorm75/ModularDoc)
