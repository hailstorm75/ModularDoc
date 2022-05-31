# IStepViewModel `interface`

## Description
Interface for view models of plugin steps

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph MarkDoc.Core
  MarkDoc.Core.IStepViewModel[[IStepViewModel]]
  class MarkDoc.Core.IStepViewModel interfaceStyle;
  MarkDoc.Core.IViewModel[[IViewModel]]
  class MarkDoc.Core.IViewModel interfaceStyle;
  end
  subgraph System
System.IDisposable[[IDisposable]]
  end
MarkDoc.Core.IViewModel --> MarkDoc.Core.IStepViewModel
System.IDisposable --> MarkDoc.Core.IViewModel
```

## Members
### Properties
#### Public  properties
| Type | Name | Methods |
| --- | --- | --- |
| `string` | [`Description`](markdoc/core/IStepViewModel.md#description)<br>Step description | `get` |
| `string` | [`Id`](markdoc/core/IStepViewModel.md#id)<br>Step view model Id | `get` |
| `bool` | [`IsValid`](markdoc/core/IStepViewModel.md#isvalid)<br>Determines whether the step form is filled correctly | `get` |
| `string` | [`Title`](markdoc/core/IStepViewModel.md#title)<br>Step name | `get` |

### Methods
#### Public  methods
| Returns | Name |
| --- | --- |
| `IReadOnlyDictionary`&lt;`string`, `string`&gt; | [`GetSettings`](markdoc/core/IStepViewModel.md#getsettings)()<br>Retrieves current settings |
| `ValueTask` | [`SetPreviousSettings`](markdoc/core/IStepViewModel.md#setprevioussettings)(`IReadOnlyDictionary`&lt;`string`, `IReadOnlyDictionary`&lt;`string`, `string`&gt;&gt; settings) |

## Details
### Summary
Interface for view models of plugin steps

### Inheritance
 - [
`IViewModel`
](./IViewModel.md)
 - `IDisposable`

### Methods
#### GetSettings
```csharp
public abstract IReadOnlyDictionary GetSettings()
```
##### Summary
Retrieves current settings

##### Returns
Current form settings

#### SetPreviousSettings
```csharp
public abstract ValueTask SetPreviousSettings(IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> settings)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `IReadOnlyDictionary`&lt;`string`, `IReadOnlyDictionary`&lt;`string`, `string`&gt;&gt; | settings |   |

### Properties
#### IsValid
```csharp
public abstract bool IsValid { get }
```
##### Summary
Determines whether the step form is filled correctly

#### Id
```csharp
public abstract string Id { get }
```
##### Summary
Step view model Id

##### Remarks
This Id is required for referencing previous settings

#### Title
```csharp
public abstract string Title { get }
```
##### Summary
Step name

#### Description
```csharp
public abstract string Description { get }
```
##### Summary
Step description

*Generated with* [*MarkDoc*](https://github.com/hailstorm75/MarkDoc.Core)
