using System.Collections.Generic;

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

    /// <summary>
    /// Child nodes
    /// </summary>
    public IReadOnlyCollection<TreeNode> Children { get; }

    /// <summary>
    /// SUMMARY
    /// </summary>
    public TreeNode? Parent { get; set; }

    /// <summary>
    /// Default constructor
    /// </summary>
    public TreeNode(string name, IReadOnlyCollection<TreeNode> children)
    {
      Name = name;
      Children = children;

      foreach (var child in children)
        child.Parent = this;
    }
  }
}