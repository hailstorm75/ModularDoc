# ILinker `interface`

## Description
Interface for creating links

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph MarkDoc.Linkers
  MarkDoc.Linkers.ILinker[[ILinker]]
  class MarkDoc.Linkers.ILinker interfaceStyle;
  end
```

## Members
### Properties
#### Public  properties
| Type | Name | Methods |
| --- | --- | --- |
| `IReadOnlyDictionary`&lt;[`IType`](./markdocmemberstypes-IType.md), `string`&gt; | [`Paths`](markdoclinkers-ILinker.md#paths)<br>Types path structure | `get` |

### Methods
#### Public  methods
| Returns | Name |
| --- | --- |
| `Lazy` | [`CreateAnchor`](markdoclinkers-ILinker.md#createanchor)([`IType`](./markdocmemberstypes-IType.md) page, [`IMember`](./markdocmembersmembers-IMember.md) member)<br>Creates an anchor to a given `member` |
| `string` | [`CreateLink`](markdoclinkers-ILinker.md#createlink-12)(`...`)<br>Creates a link to a given type `target` |
| `string` | [`CreateLinkToSourceCode`](markdoclinkers-ILinker.md#createlinktosourcecode)([`IMember`](./markdocmembersmembers-IMember.md) member) |
| `void` | [`RegisterAnchor`](markdoclinkers-ILinker.md#registeranchor)([`IMember`](./markdocmembersmembers-IMember.md) member, `Lazy`&lt;`string`&gt; anchor) |

## Details
### Summary
Interface for creating links

### Methods
#### CreateLink [1/2]
```csharp
public abstract string CreateLink(IType source, IResType target)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| [`IType`](./markdocmemberstypes-IType.md) | source | Link from |
| [`IResType`](./markdocmembersresolvedtypes-IResType.md) | target | Link target |

##### Summary
Creates a link to a given type `target`

##### Returns
Retrieved link

#### CreateLink [2/2]
```csharp
public abstract string CreateLink(IType source, IType target)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| [`IType`](./markdocmemberstypes-IType.md) | source |  |
| [`IType`](./markdocmemberstypes-IType.md) | target |  |

##### Summary
Creates a link to a given type `target`

##### Returns
Retrieved link

#### CreateAnchor
```csharp
public abstract Lazy CreateAnchor(IType page, IMember member)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| [`IType`](./markdocmemberstypes-IType.md) | page | Page type |
| [`IMember`](./markdocmembersmembers-IMember.md) | member | Member to link to |

##### Summary
Creates an anchor to a given `member`

##### Returns
Retrieved link

#### RegisterAnchor
```csharp
public abstract void RegisterAnchor(IMember member, Lazy<string> anchor)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| [`IMember`](./markdocmembersmembers-IMember.md) | member |   |
| `Lazy`&lt;`string`&gt; | anchor |   |

#### CreateLinkToSourceCode
```csharp
public abstract string CreateLinkToSourceCode(IMember member)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| [`IMember`](./markdocmembersmembers-IMember.md) | member |   |

### Properties
#### Paths
```csharp
public abstract IReadOnlyDictionary Paths { get }
```
##### Summary
Types path structure

*Generated with* [*MarkDoc*](https://github.com/hailstorm75/MarkDoc.Core)
