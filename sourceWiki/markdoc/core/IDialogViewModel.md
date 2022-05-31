# IDialogViewModel `interface`

## Description
Interface for dialog view models

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph MarkDoc.Core
  MarkDoc.Core.IDialogViewModel[[IDialogViewModel]]
  class MarkDoc.Core.IDialogViewModel interfaceStyle;
  MarkDoc.Core.IViewModel[[IViewModel]]
  class MarkDoc.Core.IViewModel interfaceStyle;
  end
  subgraph System
System.IDisposable[[IDisposable]]
  end
MarkDoc.Core.IViewModel --> MarkDoc.Core.IDialogViewModel
System.IDisposable --> MarkDoc.Core.IViewModel
```

## Members
### Properties
#### Public  properties
| Type | Name | Methods |
| --- | --- | --- |
| `bool` | [`CanClickCancel`](markdoc/core/IDialogViewModel.md#canclickcancel)<br>Determines whether the cancel button can be clicked | `get` |
| `bool` | [`CanClickNegative`](markdoc/core/IDialogViewModel.md#canclicknegative)<br>Determines whether the negative button can be clicked | `get` |
| `bool` | [`CanClickPositive`](markdoc/core/IDialogViewModel.md#canclickpositive)<br>Determines whether the positive button can be clicked | `get` |
| `string` | [`Title`](markdoc/core/IDialogViewModel.md#title)<br>Dialog title | `get` |

### Methods
#### Public  methods
| Returns | Name |
| --- | --- |
| `void` | [`OnCancelButtonClicked`](markdoc/core/IDialogViewModel.md#oncancelbuttonclicked)()<br>Invoked when the parent dialog window cancel button is pressed |
| `void` | [`OnNegativeButtonClicked`](markdoc/core/IDialogViewModel.md#onnegativebuttonclicked)()<br>Invoked when the parent dialog window negative button is pressed |
| `void` | [`OnPositiveButtonClicked`](markdoc/core/IDialogViewModel.md#onpositivebuttonclicked)()<br>Invoked when the parent dialog window positive button is pressed |

## Details
### Summary
Interface for dialog view models

### Inheritance
 - [
`IViewModel`
](./IViewModel.md)
 - `IDisposable`

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
Dialog title

#### CanClickPositive
```csharp
public abstract bool CanClickPositive { get }
```
##### Summary
Determines whether the positive button can be clicked

#### CanClickNegative
```csharp
public abstract bool CanClickNegative { get }
```
##### Summary
Determines whether the negative button can be clicked

#### CanClickCancel
```csharp
public abstract bool CanClickCancel { get }
```
##### Summary
Determines whether the cancel button can be clicked

### Events
#### CloseRequested
```csharp
public event EventHandler CloseRequested
```
##### Summary
Invoked when the dialog is to be closed

*Generated with* [*MarkDoc*](https://github.com/hailstorm75/MarkDoc.Core)
