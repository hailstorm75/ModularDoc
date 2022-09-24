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
| `string` | [`Title`](#title)<br>Dialog view title | `get` |

### Methods
#### Public  methods
| Returns | Name |
| --- | --- |
| `void` | [`OnCancelButtonClicked`](#oncancelbuttonclicked)()<br>Invoked when the parent dialog window cancel button is pressed |
| `void` | [`OnNegativeButtonClicked`](#onnegativebuttonclicked)()<br>Invoked when the parent dialog window negative button is pressed |
| `void` | [`OnPositiveButtonClicked`](#onpositivebuttonclicked)()<br>Invoked when the parent dialog window positive button is pressed |

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
public void OnPositiveButtonClicked()
```
##### Summary
Invoked when the parent dialog window positive button is pressed

#### OnNegativeButtonClicked
```csharp
public void OnNegativeButtonClicked()
```
##### Summary
Invoked when the parent dialog window negative button is pressed

#### OnCancelButtonClicked
```csharp
public void OnCancelButtonClicked()
```
##### Summary
Invoked when the parent dialog window cancel button is pressed

### Properties
#### Title
```csharp
public string Title { get; }
```
##### Summary
Dialog view title

*Generated with* [*MarkDoc*](https://github.com/hailstorm75/MarkDoc.Core)
