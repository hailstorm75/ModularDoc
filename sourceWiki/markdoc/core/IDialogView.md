# IDialogView `interface`

## Description
Interface for dialog views

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph MarkDoc.Core
  MarkDoc.Core.IDialogView[[IDialogView]]
  class MarkDoc.Core.IDialogView interfaceStyle;
  MarkDoc.Core.IView[[IView]]
  class MarkDoc.Core.IView interfaceStyle;
  end
MarkDoc.Core.IView --> MarkDoc.Core.IDialogView
```

## Members
### Properties
#### Public  properties
| Type | Name | Methods |
| --- | --- | --- |
| `string` | [`Title`](markdoc/core/IDialogView.md#title)<br>Dialog view title | `get` |

### Methods
#### Public  methods
| Returns | Name |
| --- | --- |
| `void` | [`OnCancelButtonClicked`](markdoc/core/IDialogView.md#oncancelbuttonclicked)()<br>Invoked when the parent dialog window cancel button is pressed |
| `void` | [`OnNegativeButtonClicked`](markdoc/core/IDialogView.md#onnegativebuttonclicked)()<br>Invoked when the parent dialog window negative button is pressed |
| `void` | [`OnPositiveButtonClicked`](markdoc/core/IDialogView.md#onpositivebuttonclicked)()<br>Invoked when the parent dialog window positive button is pressed |

## Details
### Summary
Interface for dialog views

### Inheritance
 - [
`IView`
](./IView.md)

### Methods
#### OnPositiveButtonClicked
```csharp
public abstract void OnPositiveButtonClicked()
```
##### Summary
Invoked when the parent dialog window positive button is pressed

#### OnNegativeButtonClicked
```csharp
public abstract void OnNegativeButtonClicked()
```
##### Summary
Invoked when the parent dialog window negative button is pressed

#### OnCancelButtonClicked
```csharp
public abstract void OnCancelButtonClicked()
```
##### Summary
Invoked when the parent dialog window cancel button is pressed

### Properties
#### Title
```csharp
public abstract string Title { get }
```
##### Summary
Dialog view title

*Generated with* [*MarkDoc*](https://github.com/hailstorm75/MarkDoc.Core)
