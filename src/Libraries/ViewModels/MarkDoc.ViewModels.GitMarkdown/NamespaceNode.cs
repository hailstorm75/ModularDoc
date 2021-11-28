using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using MarkDoc.Helpers;
using MarkDoc.Members;
using MarkDoc.Members.Types;
using ReactiveUI;

namespace MarkDoc.ViewModels.GitMarkdown
{
  public abstract class BaseTrieNode
    : ReactiveObject, IEquatable<BaseTrieNode>, IComparable<BaseTrieNode>, IComparable
  {
    #region Fields

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
    /// Display name with the namespace
    /// </summary>
    public abstract string FullName { get; }

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

    public BaseTrieNode? Parent { get; }

    #endregion

    protected BaseTrieNode(string displayName, string fullNamespace, BaseTrieNode? parentNode = default)
    {
      Parent = parentNode;
      if (Parent is not null)
        Parent.PropertyChanged += ParentOnPropertyChanged;

      DisplayName = displayName;
      FullNamespace = fullNamespace;
    }

    ~BaseTrieNode()
    {
      if (Parent is not null)
        Parent.PropertyChanged -= ParentOnPropertyChanged;
    }

    private void ParentOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
      IsEnabled = e.PropertyName switch
      {
        nameof(IsChecked) => Parent!.IsChecked,
        nameof(IsEnabled) => Parent!.IsEnabled && Parent!.IsChecked,
        _ => IsEnabled
      };
    }

    /// <inheritdoc />
    public bool Equals(BaseTrieNode? other)
    {
      if (ReferenceEquals(null, other)) return false;
      if (ReferenceEquals(this, other)) return true;
      return string.Equals(DisplayName, other.DisplayName, StringComparison.InvariantCulture) && string.Equals(FullNamespace, other.FullNamespace, StringComparison.InvariantCulture);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
      if (ReferenceEquals(null, obj)) return false;
      if (ReferenceEquals(this, obj)) return true;
      if (obj.GetType() != GetType()) return false;
      return Equals((BaseTrieNode)obj);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
      var hashCode = new HashCode();
      hashCode.Add(DisplayName, StringComparer.InvariantCulture);
      hashCode.Add(FullNamespace, StringComparer.InvariantCulture);
      return hashCode.ToHashCode();
    }

    public static bool operator ==(BaseTrieNode? left, BaseTrieNode? right) => Equals(left, right);

    public static bool operator !=(BaseTrieNode? left, BaseTrieNode? right) => !Equals(left, right);

    /// <inheritdoc />
    public int CompareTo(BaseTrieNode? other)
    {
      if (ReferenceEquals(this, other)) return 0;
      if (ReferenceEquals(null, other)) return 1;
      var displayNameComparison = string.Compare(DisplayName, other.DisplayName, StringComparison.InvariantCulture);
      if (displayNameComparison != 0) return displayNameComparison;
      return string.Compare(FullNamespace, other.FullNamespace, StringComparison.InvariantCulture);
    }

    /// <inheritdoc />
    public int CompareTo(object? obj)
    {
      if (ReferenceEquals(null, obj)) return 1;
      if (ReferenceEquals(this, obj)) return 0;
      return obj is BaseTrieNode other ? CompareTo(other) : throw new ArgumentException($"Object must be of type {nameof(BaseTrieNode)}");
    }
  }

  [DebuggerDisplay(nameof(TypeNode) + (": {" + nameof(DisplayName) + "}"))]
  public class TypeNode
    : BaseTrieNode
  {
    private static readonly Regex GENERICS_REGEX = new Regex(@"`\d+");

    /// <inheritdoc />
    public override string FullName => WrappedType.RawName;

    /// <summary>
    /// Wrapped type instance
    /// </summary>
    public IType WrappedType { get; }

    public IReadOnlyCollection<TypeNode> TypeNodes { get; }

    /// <summary>
    /// Default constructor
    /// </summary>
    public TypeNode(IType type, BaseTrieNode? parent = default)
      : base(type.Name, GetFullNamespace(type), parent)
    {
      WrappedType = type;

      if (type is IInterface interfaceType)
        TypeNodes = interfaceType.NestedTypes.Select(nestedType => new TypeNode(nestedType, this)).ToReadOnlyCollection();
      else
        TypeNodes = Array.Empty<TypeNode>();
    }

    public IEnumerable<TypeNode> GetAllSubNodes()
      => TypeNodes.Concat(TypeNodes.SelectMany(node => node.GetAllSubNodes()));

    private static string GetFullNamespace(IType child)
      => GENERICS_REGEX.Replace($"{child.TypeNamespace}.{child.Name}", string.Empty);
  }

  [DebuggerDisplay(nameof(NamespaceNode) + (": {" + nameof(DisplayName) + "}"))]
  public class NamespaceNode
    : BaseTrieNode
  {
    #region Fields

    private readonly Dictionary<string, BaseTrieNode> m_nodes = new();

    #endregion

    #region Properties

    /// <inheritdoc />
    public override string FullName => FullNamespace;

    /// <summary>
    /// Child nodes
    /// </summary>
    public IEnumerable<BaseTrieNode> Nodes => m_nodes.Values;

    #endregion

    private NamespaceNode(string displayName, string fullNamespace, NamespaceNode? parent = default)
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

        var root = new NamespaceNode(nodes.First(), nodes.First());
        var typeNodes = types is null
          ? Array.Empty<TypeNode>()
          // ReSharper disable once AccessToModifiedClosure
          : types.Select(type => new TypeNode(type, root)).ToReadOnlyCollection();
        foreach (var typeNode in typeNodes)
        {
          if (!typeNode.WrappedType.IsNested)
            root.AddType(typeNode);

          allNodes.AddLast(typeNode);
        }

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
          {
            allNodes.AddLast(child);

            resolver.Types.Value.TryGetValue(fullNamespace, out types);
            typeNodes = types is null
              ? Array.Empty<TypeNode>()
              : types.Select(type => new TypeNode(type, child)).ToReadOnlyCollection();
            foreach (var typeNode in typeNodes)
            {
              if (typeNode.WrappedType.IsNested)
                continue;

              child.AddType(typeNode);
              allNodes.AddLast(typeNode);

              var childSubNodes = typeNode.GetAllSubNodes();
              foreach (var subNode in childSubNodes)
                allNodes.AddLast(subNode);
            }
          }
          parent = existing;
          index++;
        }
      }

      return (roots.Values, allNodes);
    }
  }
}