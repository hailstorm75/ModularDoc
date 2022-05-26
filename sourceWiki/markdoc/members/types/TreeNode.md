# TreeNode `class`

## Description
Node of a tree structure

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph MarkDoc.Members.Types
  MarkDoc.Members.Types.TreeNode[[TreeNode]]
  end
```

## Members
### Properties
#### Public  properties
| Type | Name | Methods |
| --- | --- | --- |
| `IReadOnlyCollection`&lt;[`TreeNode`](markdoc/members/types/TreeNode.md)&gt; | [`Children`](markdoc/members/types/TreeNode.md#children)<br>Child nodes | `get` |
| `string` | [`Name`](markdoc/members/types/TreeNode.md#name)<br>Node name | `get` |
| [`IResType`](../resolvedtypes/IResType.md) | [`Value`](markdoc/members/types/TreeNode.md#value) | `get` |

## Details
### Summary
Node of a tree structure

### Constructors
#### TreeNode
[*Source code*](https://github.com///blob//src/Libraries/Core/MarkDoc.Members/Types/TreeNode.cs#L26)
```csharp
public TreeNode(string name, IResType value, IReadOnlyCollection<TreeNode> children)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `string` | name |   |
| [`IResType`](../resolvedtypes/IResType.md) | value |   |
| `IReadOnlyCollection`&lt;[`TreeNode`](markdoc/members/types/TreeNode.md)&gt; | children |   |

### Properties
#### Name
```csharp
public string Name { get }
```
##### Summary
Node name

#### Value
```csharp
public IResType Value { get }
```

#### Children
```csharp
public IReadOnlyCollection Children { get }
```
##### Summary
Child nodes

*Generated with* [*MarkDoc*](https://github.com/hailstorm75/MarkDoc.Core)
