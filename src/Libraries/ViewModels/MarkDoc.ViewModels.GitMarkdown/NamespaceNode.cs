using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using MarkDoc.Helpers;
using MarkDoc.Members;
using MarkDoc.Members.Types;
using ReactiveUI;

namespace MarkDoc.ViewModels.GitMarkdown
{
  public abstract class BaseTrieNode
    : ReactiveObject
  {
    #region Fields

    private readonly BaseTrieNode? m_parent;
    private bool m_isChecked = true;
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

    #endregion

    protected BaseTrieNode(string displayName, string fullNamespace, BaseTrieNode? parentNode = default)
    {
      m_parent = parentNode;
      if (m_parent is not null)
        m_parent.PropertyChanged += ParentOnPropertyChanged;

      DisplayName = displayName;
      FullNamespace = fullNamespace;
    }

    ~BaseTrieNode()
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
  }

  public class TypeNode
    : BaseTrieNode
  {
    private static readonly Regex GENERICS_REGEX = new Regex(@"`\d+");

    public IReadOnlyCollection<TypeNode> TypeNodes { get; }

    /// <summary>
    /// Default constructor
    /// </summary>
    public TypeNode(IType type, BaseTrieNode? parent = default)
      : base(type.Name, GetFullNamespace(type), parent)
    {
      if (type is IInterface interfaceType)
        TypeNodes = interfaceType.NestedTypes.Select(nestedType => new TypeNode(nestedType, this)).ToReadOnlyCollection();
      TypeNodes = Array.Empty<TypeNode>();
    }

    private static string GetFullNamespace(IType child)
      => GENERICS_REGEX.Replace($"{child.TypeNamespace}.{child.Name}", string.Empty);
  }

  public class NamespaceNode
    : BaseTrieNode
  {
    #region Fields

    private readonly Dictionary<string, BaseTrieNode> m_nodes = new();

    #endregion

    #region Properties

    /// <summary>
    /// Child nodes
    /// </summary>
    public IEnumerable<BaseTrieNode> Nodes => m_nodes.Values;

    #endregion

    private NamespaceNode(string displayName, string fullNamespace, IReadOnlyCollection<IType>? types, NamespaceNode? parent = default)
      : base(displayName, fullNamespace, parent)
    {
    }

    private bool TryAdd(NamespaceNode node, out NamespaceNode existing)
    {
      if (!m_nodes.TryAdd(node.DisplayName, node) && m_nodes[node.DisplayName] is NamespaceNode namespaceNode)
      {
        existing = namespaceNode;
        return false;
      }

      existing = node;

      return true;
    }

    private void AddType(TypeNode node)
    {
      m_nodes.TryAdd(node.DisplayName, node);
    }

    public static (IReadOnlyCollection<NamespaceNode> roots, IReadOnlyCollection<BaseTrieNode> allNodes) CreateNodes(IResolver resolver)
    {
      var roots = new Dictionary<string, NamespaceNode>();
      var allNodes = new LinkedList<BaseTrieNode>();

      foreach (var @namespace in resolver.Types.Value.Keys)
      {
        var nodes = @namespace.Split('.');

        resolver.Types.Value.TryGetValue(nodes.First(), out var types);

        var root = new NamespaceNode(nodes.First(), nodes.First(), types);
        var typeNodes = types is null
          ? Array.Empty<TypeNode>()
          : types.Select(nestedType => new TypeNode(nestedType, root)).ToReadOnlyCollection();
        foreach (var typeNode in typeNodes)
          root.AddType(typeNode);

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

          var child = new NamespaceNode(displayName, fullNamespace, types, parent);
          resolver.Types.Value.TryGetValue(fullNamespace, out types);
          typeNodes = types is null
            ? Array.Empty<TypeNode>()
            : types.Select(nestedType => new TypeNode(nestedType, child)).ToReadOnlyCollection();
          foreach (var typeNode in typeNodes)
            child.AddType(typeNode);

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