# IResType `interface`

## Description
Interface for resolved types

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph MarkDoc.Members.ResolvedTypes
  MarkDoc.Members.ResolvedTypes.IResType[[IResType]]
  class MarkDoc.Members.ResolvedTypes.IResType interfaceStyle;
  end
```

## Members
### Properties
#### Public  properties
| Type | Name | Methods |
| --- | --- | --- |
| `string` | [`DisplayName`](markdocmembersresolvedtypes-IResType#displayname)<br>Resolved type display name | `get` |
| `string` | [`DocumentationName`](markdocmembersresolvedtypes-IResType#documentationname)<br>Resolved type name for documentation | `get` |
| `bool` | [`IsByRef`](markdocmembersresolvedtypes-IResType#isbyref)<br>Is the type a reference | `get` |
| `string` | [`RawName`](markdocmembersresolvedtypes-IResType#rawname)<br>Resolved type raw name | `get` |
| `Lazy`&lt;[`IType`](./markdocmemberstypes-IType)&gt; | [`Reference`](markdocmembersresolvedtypes-IResType#reference)<br>Reference to known type | `get` |
| `string` | [`TypeNamespace`](markdocmembersresolvedtypes-IResType#typenamespace)<br>Resolved type namespace | `get` |

## Details
### Summary
Interface for resolved types

### Properties
#### DisplayName
```csharp
public abstract string DisplayName { get }
```
##### Summary
Resolved type display name

#### DocumentationName
```csharp
public abstract string DocumentationName { get }
```
##### Summary
Resolved type name for documentation

#### RawName
```csharp
public abstract string RawName { get }
```
##### Summary
Resolved type raw name

#### TypeNamespace
```csharp
public abstract string TypeNamespace { get }
```
##### Summary
Resolved type namespace

#### Reference
```csharp
public abstract Lazy Reference { get }
```
##### Summary
Reference to known type

#### IsByRef
```csharp
public abstract bool IsByRef { get }
```
##### Summary
Is the type a reference

*Generated with* [*MarkDoc*](https://github.com/hailstorm75/MarkDoc.Core)
