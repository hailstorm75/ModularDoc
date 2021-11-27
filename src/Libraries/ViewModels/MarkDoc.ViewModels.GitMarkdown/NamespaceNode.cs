using System.Collections.Generic;
using System.Linq;
using MarkDoc.Helpers;
using MarkDoc.Members;
using ReactiveUI;

namespace MarkDoc.ViewModels.GitMarkdown
{
  public class NamespaceNode
    : ReactiveObject
  {
    #region Fields

    private NamespaceNode? m_parent;
    private bool m_isChecked;
    private bool m_isExpanded;

    #endregion

    #region Properties

    /// <summary>
    /// Namespace part name
    /// </summary>
    public string DisplayName { get; }

    /// <summary>
    /// Full namespace path
    /// </summary>
    public string FullNamespace { get; }

    /// <summary>
    /// Determines whether this namespace part is enabled
    /// </summary>
    public bool IsChecked
    {
      get => m_isChecked;
      set
      {
        m_isChecked = value;
        this.RaisePropertyChanged(nameof(IsChecked));
      }
    }

    /// <summary>
    /// Determines whether this namespace node is expanded
    /// </summary>
    public bool IsExpanded
    {
      get => m_isExpanded;
      set
      {
        m_isExpanded = value;
        this.RaisePropertyChanged(nameof(IsExpanded));
      }
    }

    /// <summary>
    /// Determines whether the parent namespace is enabled
    /// </summary>
    public bool IsParentChecked => m_parent?.IsChecked ?? true;

    /// <summary>
    /// Child nodes
    /// </summary>
    public IReadOnlyCollection<NamespaceNode> Nodes { get; }

    #endregion

    private NamespaceNode(string displayName, string fullNamespace, IEnumerable<NamespaceNode> nodes, NamespaceNode? parentNode = default)
    {
      m_parent = parentNode;

      DisplayName = displayName;
      FullNamespace = fullNamespace;
      Nodes = nodes.ToReadOnlyCollection();
    }

    public static (IReadOnlyCollection<NamespaceNode> roots, IReadOnlyCollection<NamespaceNode> allNodes) CreateNodes(IResolver resolver)
    {
      var roots = new Dictionary<string, NamespaceNode>();
      var allNodes = new LinkedList<NamespaceNode>();

      foreach (var @namespace in resolver.Types.Value.Keys)
      {
        var nodes = @namespace.Split('.');
        var childNodes = new Dictionary<string, NamespaceNode>();

        var root = new NamespaceNode(nodes.First(), nodes.First(), childNodes.Values);
        roots.TryAdd(nodes.First(), root);

        foreach (var childNamespace in nodes.Skip(1))
        {
          
        }

        allNodes.AddLast(root);
      }

      return (roots.Values, allNodes);
    }
  }
}