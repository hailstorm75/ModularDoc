using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using MarkDoc.Members;
using ReactiveUI;

namespace MarkDoc.ViewModels.GitMarkdown
{
  public class NamespaceNode
    : ReactiveObject
  {
    #region Fields

    private readonly NamespaceNode? m_parent;
    private bool m_isChecked = true;
    private readonly Dictionary<string, NamespaceNode> m_nodes = new();
    private bool m_isEnabled = true;

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
    /// Determines whether this node is enabled for editing
    /// </summary>
    public bool IsEnabled
    {
      get => m_isEnabled;
      set
      {
        m_isEnabled = value;
        this.RaisePropertyChanged(nameof(IsEnabled));
      }
    }

    /// <summary>
    /// Child nodes
    /// </summary>
    public IReadOnlyCollection<NamespaceNode> Nodes => m_nodes.Values;

    #endregion

    private NamespaceNode(string displayName, string fullNamespace, NamespaceNode? parentNode = default)
    {
      m_parent = parentNode;
      if (m_parent is not null)
        m_parent.PropertyChanged += ParentOnPropertyChanged;

      DisplayName = displayName;
      FullNamespace = fullNamespace;
    }

    ~NamespaceNode()
    {
      if (m_parent is not null)
        m_parent.PropertyChanged -= ParentOnPropertyChanged;
    }

    private void ParentOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
      IsEnabled = e.PropertyName switch
      {
        nameof(IsChecked) => m_parent!.IsChecked,
        nameof(IsEnabled) => m_parent!.IsEnabled,
        _ => IsEnabled
      };
    }

    private bool TryAdd(NamespaceNode node, out NamespaceNode existing)
    {
      if (!m_nodes.TryAdd(node.DisplayName, node))
      {
        existing = m_nodes[node.DisplayName];
        return false;
      }

      existing = node;

      return true;
    }

    public static (IReadOnlyCollection<NamespaceNode> roots, IReadOnlyCollection<NamespaceNode> allNodes) CreateNodes(IResolver resolver)
    {
      var roots = new Dictionary<string, NamespaceNode>();
      var allNodes = new LinkedList<NamespaceNode>();

      foreach (var @namespace in resolver.Types.Value.Keys)
      {
        var nodes = @namespace.Split('.');

        var root = new NamespaceNode(nodes.First(), nodes.First());
        if (roots.TryAdd(nodes.First(), root))
          allNodes.AddLast(root);
        // Otherwise..
        else
          root = roots[nodes.First()];

        var index = 1;
        var parent = root;
        while (true)
        {
          if (index == nodes.Length)
            break;

          var displayName = nodes[index];
          var fullNamespace = string.Join('.', nodes.Take(index + 1));

          var child = new NamespaceNode(displayName, fullNamespace, parent);
          if (parent.TryAdd(child, out var existing))
            allNodes.AddLast(child);

          parent = existing;
          index++;
        }
      }

      return (roots.Values, allNodes);
    }
  }
}