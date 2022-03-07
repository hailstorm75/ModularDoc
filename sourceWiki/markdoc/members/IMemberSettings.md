# IMemberSettings `interface`

## Description
Interface for [IResolver](./IResolver.md) settings

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph MarkDoc.Members
  MarkDoc.Members.IMemberSettings[[IMemberSettings]]
  class MarkDoc.Members.IMemberSettings interfaceStyle;
  end
  subgraph MarkDoc.Core
  MarkDoc.Core.ILibrarySettings[[ILibrarySettings]]
  class MarkDoc.Core.ILibrarySettings interfaceStyle;
  end
MarkDoc.Core.ILibrarySettings --> MarkDoc.Members.IMemberSettings
```

## Members
### Properties
#### Public  properties
| Type | Name | Methods |
| --- | --- | --- |
| `IReadOnlyCollection`&lt;`string`&gt; | [`Paths`](markdoc/members/IMemberSettings.md#paths)<br>Paths to member sources | `get` |

## Details
### Summary
Interface for [IResolver](./IResolver.md) settings

### Inheritance
 - [
`ILibrarySettings`
](../core/ILibrarySettings.md)

### Properties
#### Paths
```csharp
public abstract IReadOnlyCollection Paths { get }
```
##### Summary
Paths to member sources

*Generated with* [*MarkDoc*](https://github.com/hailstorm75/MarkDoc.Core)
