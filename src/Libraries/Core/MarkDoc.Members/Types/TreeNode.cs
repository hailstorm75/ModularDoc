using System.Collections.Generic;
using MarkDoc.Members.ResolvedTypes;

namespace MarkDoc.Members.Types
{
  /// <summary>
  /// Node of a tree structure
  /// </summary>
  public class TreeNode
  {
    /// <summary>
    /// Node name
    /// </summary>
    public string Name { get; }

    public IResType Value { get;}

    /// <summary>
    /// Child nodes
    /// </summary>
    public IReadOnlyCollection<TreeNode> Children { get; }

    /// <summary>
    /// Default constructor
    /// </summary>
    public TreeNode(string name, IResType value, IReadOnlyCollection<TreeNode> children)
    {
      Name = name;
      Value = value;
      Children = children;
    }
  }
}