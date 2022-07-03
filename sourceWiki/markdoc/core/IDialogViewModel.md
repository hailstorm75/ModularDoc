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
| `bool` | [`CanClickCancel`](#canclickcancel)<br>Determines whether the cancel button can be clicked | `get` |
| `bool` | [`CanClickNegative`](#canclicknegative)<br>Determines whether the negative button can be clicked | `get` |
| `bool` | [`CanClickPositive`](#canclickpositive)<br>Determines whether the positive button can be clicked | `get` |
| `string` | [`Title`](#title)<br>Dialog title | `get` |

### Methods
#### Public  methods
| Returns | Name |
| --- | --- |
| `void` | [`OnCancelButtonClicked`](#oncancelbuttonclicked)()<br>Invoked when the parent dialog window cancel button is pressed |
| `void` | [`OnNegativeButtonClicked`](#onnegativebuttonclicked)()<br>Invoked when the parent dialog window negative button is pressed |
| `void` | [`OnPositiveButtonClicked`](#onpositivebuttonclicked)()<br>Invoked when the parent dialog window positive button is pressed |

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
Dialog title

#### CanClickPositive
```csharp
public bool CanClickPositive { get; }
```
##### Summary
Determines whether the positive button can be clicked

#### CanClickNegative
```csharp
public bool CanClickNegative { get; }
```
##### Summary
Determines whether the negative button can be clicked

#### CanClickCancel
```csharp
public bool CanClickCancel { get; }
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
