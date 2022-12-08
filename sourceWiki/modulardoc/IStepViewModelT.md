# IStepViewModel&lt; TSettings&gt; `interface`

## Description
Interface for view models of plugin steps

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

  ModularDoc.IStepViewModel[[IStepViewModel]]
  class ModularDoc.IStepViewModel interfaceStyle;
  ModularDoc.IViewModel[[IViewModel]]
  class ModularDoc.IViewModel interfaceStyle;
  end
  subgraph System
System.IDisposable[[IDisposable]]
  end
ModularDoc.IStepViewModel --> ModularDoc.IStepViewModel_1
ModularDoc.IViewModel --> ModularDoc.IStepViewModel
System.IDisposable --> ModularDoc.IViewModel
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
