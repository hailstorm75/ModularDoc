# ISettingsCreator `interface`

## Description
Interface for settings creators

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph ModularDoc.Core
  ModularDoc.Core.ISettingsCreator[[ISettingsCreator]]
  class ModularDoc.Core.ISettingsCreator interfaceStyle;
  end
```

## Members
### Methods
#### Public  methods
| Returns | Name |
| --- | --- |
| `T` | [`CreateSettings`](#createsettings)(`IReadOnlyDictionary`&lt;`string`, `string`&gt; data) |

## Details
### Summary
Interface for settings creators

### Methods
#### CreateSettings
```csharp
public T CreateSettings<T>(IReadOnlyDictionary<string, string> data)
where T : ILibrarySettings
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `IReadOnlyDictionary`&lt;`string`, `string`&gt; | data |   |

*Generated with* [*ModularDoc*](https://github.com/hailstorm75/ModularDoc)
