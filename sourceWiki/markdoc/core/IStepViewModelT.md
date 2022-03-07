# IStepViewModel&lt; TSettings&gt; `interface`

## Description
Interface for view models of plugin steps

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

  MarkDoc.Core.IStepViewModel[[IStepViewModel]]
  class MarkDoc.Core.IStepViewModel interfaceStyle;
  MarkDoc.Core.IViewModel[[IViewModel]]
  class MarkDoc.Core.IViewModel interfaceStyle;
  end
  subgraph System
System.IDisposable[[IDisposable]]
  end
MarkDoc.Core.IStepViewModel --> MarkDoc.Core.IStepViewModel_1
MarkDoc.Core.IViewModel --> MarkDoc.Core.IStepViewModel_1
System.IDisposable --> MarkDoc.Core.IStepViewModel_1
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

*Generated with* [*MarkDoc*](https://github.com/hailstorm75/MarkDoc.Core)
